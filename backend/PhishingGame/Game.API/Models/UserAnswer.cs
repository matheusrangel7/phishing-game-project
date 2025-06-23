namespace Game.API.Models;

public class UserAnswer
{
    public int EmailId { get; set; }       // ID do email respondido
    public bool? UserIsPhishing { get; set; } // Resposta do usuário (true = phishing)
}