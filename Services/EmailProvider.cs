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
                    new Email("contato@netflix.com", "Problemas com seu pagamento", 
                    "Não foi possível processar seu pagamento. Atualize seus dados clicando aqui."),
                    true),

                new EmailPhishingOrNot(
                    new Email("no-reply@github.com", "Nova atividade suspeita", 
                    "Foi detectado um novo login em sua conta GitHub. Verifique se foi você."),
                    false),

                new EmailPhishingOrNot(
                    new Email("rh@empresa.com", "Revisão salarial anual", 
                    "Prezados colaboradores, iniciaremos a revisão salarial a partir da próxima semana."),
                    false),

                new EmailPhishingOrNot(
                    new Email("suporte@paypal.com", "Sua conta foi limitada", 
                    "Clique aqui para confirmar suas informações e restaurar o acesso."),
                    true),

                new EmailPhishingOrNot(
                    new Email("amigo.pessoal@email.com", "Fotos da viagem", 
                    "Oi! Aqui estão as fotos daquela viagem que fizemos ano passado :)"),
                    false),

                new EmailPhishingOrNot(
                    new Email("marketing@lojaonline.com", "50% de desconto em todos os produtos!", 
                    "Aproveite nossa promoção por tempo limitado. Não perca!"),
                    false),

                new EmailPhishingOrNot(
                    new Email("atendimento@itau.com.br", "Transação pendente", 
                    "Identificamos uma transação incomum em sua conta. Acesse seu app para detalhes."),
                    false),

                new EmailPhishingOrNot(
                    new Email("itau@seguranca-alerta.com", "Alerta de transação não autorizada", 
                    "Clique aqui para cancelar uma compra que você não fez."),
                    true),

                new EmailPhishingOrNot(
                    new Email("diretoria@empresa.com", "Mudança na política de reembolso", 
                    "A nova política entrará em vigor no próximo mês. Confira os detalhes no portal."),
                    false),

                new EmailPhishingOrNot(
                    new Email("recrutamento@linkedin-careers.net", "Vaga disponível para você!", 
                    "Identificamos uma vaga com seu perfil. Acesse o link para mais detalhes."),
                    true),

                new EmailPhishingOrNot(
                    new Email("info@google.com", "Confirmação de recuperação de senha", 
                    "Você solicitou a recuperação de senha? Caso não tenha sido você, ignore esta mensagem."),
                    false),

                new EmailPhishingOrNot(
                    new Email("admin@google-security.com", "Atualize seu login", 
                    "Evite bloqueio: faça login aqui e confirme sua identidade."),
                    true),

                new EmailPhishingOrNot(
                    new Email("supervisor@empresa.com", "Entrega do projeto", 
                    "Lembrete: o projeto precisa ser entregue até sexta às 17h."),
                    false),

                new EmailPhishingOrNot(
                    new Email("apple@fatura-email.com", "Sua assinatura foi renovada", 
                    "Recebemos o pagamento de R$129,90 pela assinatura Apple Music. Cancelar agora."),
                    true),

                new EmailPhishingOrNot(
                    new Email("servico@receita.gov.br", "Pendência no CPF", 
                    "Verificamos uma inconsistência em seu CPF. Acesse o portal e regularize sua situação."),
                    true),

                new EmailPhishingOrNot(
                    new Email("eventos@universidade.edu", "Palestra com convidado internacional", 
                    "Participe da palestra sobre segurança digital nesta quinta-feira às 19h."),
                    false),

                new EmailPhishingOrNot(
                    new Email("notificacoes@whatsapp.com", "Seu backup foi restaurado", 
                    "Seu histórico foi restaurado com sucesso em um novo dispositivo."),
                    false),

                new EmailPhishingOrNot(
                    new Email("whatsapp@verificacao-falsa.com", "Ação necessária!", 
                    "Clique aqui para manter sua conta ativa no WhatsApp."),
                    true),

                new EmailPhishingOrNot(
                    new Email("contato@mercadolivre.com", "Compra cancelada", 
                    "Sua compra foi cancelada com sucesso. O reembolso será realizado em até 5 dias úteis."),
                    false),

                new EmailPhishingOrNot(
                    new Email("suporte@ml-fraude.com", "Problemas com seu pedido", 
                    "Para resolver o problema, clique no link abaixo e atualize seu endereço."),
                    true),
            };

            return emails.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
