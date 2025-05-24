namespace PhishingGameWebTest.Models
{
    public class Email
    {
        public string Remetente { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }
        
        public Email(string remetente, string assunto, string corpo)
        {
            Remetente = remetente;
            Assunto = assunto;
            Corpo = corpo;
        }
    }
}