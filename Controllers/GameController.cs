using Microsoft.AspNetCore.Mvc;
using PhishingGameWebTest.Models;
using PhishingGameWebTest.Services;
using System.Text.Json;

namespace PhishingGameWebTest.Controllers
{
    public class GameController : Controller
    {
        private const string SessionKeyEmails = "Emails";
        private const string SessionKeyScore = "Score";
        private const string SessionKeyIndex = "Index";

        public IActionResult Index(string? resposta = null)
        {
            List<EmailPhishingOrNot> emails;
            var emailsJson = HttpContext.Session.GetString(SessionKeyEmails);

            if (string.IsNullOrEmpty(emailsJson))
            {
                emails = EmailProvider.GetEmails();
                HttpContext.Session.SetString(SessionKeyEmails, JsonSerializer.Serialize(emails));
                HttpContext.Session.SetInt32(SessionKeyScore, 0);
                HttpContext.Session.SetInt32(SessionKeyIndex, 0);
            }
            else
            {
                emails = JsonSerializer.Deserialize<List<EmailPhishingOrNot>>(emailsJson) ?? new List<EmailPhishingOrNot>();
            }

            int score = HttpContext.Session.GetInt32(SessionKeyScore) ?? 0;
            int index = HttpContext.Session.GetInt32(SessionKeyIndex) ?? 0;

            if (!string.IsNullOrEmpty(resposta) && index < emails.Count)
            {
                bool userAnswer = resposta == "true";

                if (emails[index].VerificarResposta(userAnswer))
                {
                    TempData["Resultado"] = "✅ Resposta correta!";
                    score++;
                }
                else
                {
                    TempData["Resultado"] = "❌ Resposta incorreta.";
                }

                index++;

                HttpContext.Session.SetString(SessionKeyEmails, JsonSerializer.Serialize(emails));
                HttpContext.Session.SetInt32(SessionKeyScore, score);
                HttpContext.Session.SetInt32(SessionKeyIndex, index);

                return RedirectToAction("Index");
            }

            if (index >= emails.Count)
            {
                ViewBag.Final = true;
                ViewBag.Score = score;

                HttpContext.Session.Remove(SessionKeyEmails);
                HttpContext.Session.Remove(SessionKeyScore);
                HttpContext.Session.Remove(SessionKeyIndex);

                return View();
            }

            ViewBag.EmailAtual = emails[index];
            ViewBag.Score = score;
            ViewBag.Resultado = TempData["Resultado"];

            return View();
        }
    }
}
