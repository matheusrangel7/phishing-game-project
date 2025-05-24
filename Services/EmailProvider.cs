using PhishingGameWebTest.Models;

namespace PhishingGameWebTest.Services
{
    public static class EmailProvider
    {
        public static List<EmailPhishingOrNot> GetEmails()
        {
            var emails = new List<EmailPhishingOrNot>
            {
                new EmailPhishingOrNot(
                    new Email("newsletter@empresa.com", "Boletim Semanal", "Obrigado por se inscrever no nosso boletim semanal. Até a próxima edição!"),
                    false),
                
                new EmailPhishingOrNot(
                    new Email("seguranca@site.com", "Alerta de login", "Detectamos um login suspeito. Acesse este link para verificar sua conta."),
                    true),

                new EmailPhishingOrNot(
                    new Email("colega@empresa.com", "Relatório da reunião", "Olá, segue em anexo o relatório solicitado na reunião de hoje cedo."),
                    false),

                new EmailPhishingOrNot(
                    new Email("loteria@ganhouagora.com", "Você ganhou!", "Você ganhou na loteria! Clique aqui para resgatar seu prêmio agora."),
                    true),

                new EmailPhishingOrNot(
                    new Email("suporte@banco.com", "Atualização de conta", "Aviso: sua conta será atualizada com novos recursos na próxima semana."),
                    false),

                new EmailPhishingOrNot(
                    new Email("banco@fraude.com", "URGENTE: bloqueio de conta", "Atualize suas informações bancárias imediatamente para evitar o bloqueio da conta."),
                    true),

                new EmailPhishingOrNot(
                    new Email("vasco@futebol.com", "Vasco campeão!", "Vasco campeão do Mundial de 2000"),
                    true)
                
            };

            return emails.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}