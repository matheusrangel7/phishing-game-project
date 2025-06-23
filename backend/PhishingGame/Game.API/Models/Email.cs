namespace Game.API.Models;

public class Email
{
    public int Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Easy";
    public bool IsPhishing { get; set; }
    public string Explanation { get; set; } = string.Empty;
}