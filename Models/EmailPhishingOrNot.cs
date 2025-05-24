namespace PhishingGameWebTest.Models
{
    public class EmailPhishingOrNot
    {
        public Email Email { get; set; }
        public bool IsPhishing { get; set; }

        public EmailPhishingOrNot(Email email, bool isPhishing)
        {
            Email = email;
            IsPhishing = isPhishing;
        }

        public bool VerificarResposta(bool respostaUsuario)
        {
            return IsPhishing == respostaUsuario;
        }
    }
}