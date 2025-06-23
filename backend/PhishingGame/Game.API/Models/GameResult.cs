namespace Game.API.Models;

public class GameResult
{
    public Guid SessionId { get; set; }
    public int Score { get; set; }
    public int TotalPossible { get; set; }
    public int CorrectAnswers { get; set; }
    public int Unanswered { get; set; }
    public int TimeTaken { get; set; }
    public List<AnswerDetail> Details { get; set; } = new List<AnswerDetail>();
}

public class AnswerDetail
{
    public int EmailId { get; set; }
    public bool? WasCorrect { get; set; }
    public bool WasAnswered { get; set; }
    public string Explanation { get; set; } = string.Empty;
}