namespace Game.API.Models;

public class GameSettings
{
    public string Difficulty { get; set; } = "Easy";
    public int EmailCount { get; set; } = 5;
    public int TotalTimeSeconds { get; set; } = 60;
}