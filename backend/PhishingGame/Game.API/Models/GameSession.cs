namespace Game.API.Models;

public class GameSession
{
    public Guid SessionId { get; set; }
    public List<Email> Emails { get; set; } = new List<Email>();
    public DateTime StartTime { get; set; }
    public int TotalTimeSeconds { get; set; } = 90;
    public double TimePerEmail { get; set; }
}