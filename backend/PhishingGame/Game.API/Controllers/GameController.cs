using Game.API.Data;
using Game.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<GameController> _logger;
    private static readonly Dictionary<Guid, GameSession> ActiveSessions = new();

    public GameController(AppDbContext context, ILogger<GameController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("start")]
    public async Task<ActionResult<GameSession>> StartGame([FromBody] GameSettings settings)
    {
        try
        {
            var (emailCount, timePerEmail) = GetGameSettings(settings.Difficulty);
            
            var allEmails = await _context.Emails.ToListAsync();
            
            var filteredEmails = settings.Difficulty == "PhishingMaster" 
                ? allEmails
                : allEmails
                    .Where(e => e.Difficulty.Equals(settings.Difficulty, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            
            if (filteredEmails.Count < emailCount)
            {
                return BadRequest($"Não há e-mails suficientes. Disponíveis: {filteredEmails.Count}, solicitados: {emailCount}");
            }

            var rnd = new Random();
            var selectedEmails = filteredEmails
                .OrderBy(e => rnd.Next())
                .Take(emailCount)
                .ToList();

            var session = new GameSession
            {
                SessionId = Guid.NewGuid(),
                Emails = selectedEmails,
                StartTime = DateTime.UtcNow,
                TotalTimeSeconds = 90,
                TimePerEmail = timePerEmail
            };

            ActiveSessions[session.SessionId] = session;
            return Ok(session);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao iniciar jogo");
            return StatusCode(500, "Erro interno ao iniciar o jogo");
        }
    }

    private (int emailCount, double timePerEmail) GetGameSettings(string difficulty)
    {
        return difficulty switch
        {
            "Easy" => (5, 18.0),
            "Medium" => (6, 15.0),
            "Hard" => (7, 12.85),
            "PhishingMaster" => (15, 6.0),
            _ => (5, 18.0)
        };
    }

    [HttpPost("submit")]
    public ActionResult<GameResult> SubmitAnswers(
        [FromBody] List<UserAnswer> answers,
        [FromQuery] Guid sessionId)
    {
        if (!ActiveSessions.TryGetValue(sessionId, out var session))
            return NotFound("Sessão de jogo não encontrada ou expirada");

        var result = new GameResult
        {
            SessionId = sessionId,
            TimeTaken = (int)(DateTime.UtcNow - session.StartTime).TotalSeconds,
            Unanswered = 0
        };

        foreach (var userAnswer in answers)
        {
            var email = session.Emails.FirstOrDefault(e => e.Id == userAnswer.EmailId);
            if (email == null) continue;

            if (userAnswer.UserIsPhishing == null)
            {
                result.Unanswered++;
                result.Details.Add(new AnswerDetail
                {
                    EmailId = email.Id,
                    WasAnswered = false,
                    Explanation = email.Explanation // CORREÇÃO AQUI: usar a explicação original
                });
                continue;
            }

            var isCorrect = email.IsPhishing == userAnswer.UserIsPhishing;
            if (isCorrect)
            {
                result.Score += GetPointsForDifficulty(email.Difficulty);
                result.CorrectAnswers++;
            }

            result.Details.Add(new AnswerDetail
            {
                EmailId = email.Id,
                WasCorrect = isCorrect,
                WasAnswered = true,
                Explanation = email.Explanation
            });
        }

        result.TotalPossible = session.Emails
            .Where(e => answers.Any(a => a.EmailId == e.Id && a.UserIsPhishing != null))
            .Sum(e => GetPointsForDifficulty(e.Difficulty));
    
        ActiveSessions.Remove(sessionId);
        return Ok(result);
    }

    private int GetPointsForDifficulty(string difficulty)
    {
        return difficulty switch
        {
            "Easy" => 10,
            "Medium" => 20,
            "Hard" => 30,
            _ => 10
        };
    }
}