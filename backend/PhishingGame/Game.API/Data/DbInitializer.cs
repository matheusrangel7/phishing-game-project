using Game.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.API.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Emails.Any())
            return;

        var random = new Random();
        var emails = new List<Email>();
        var domains = new[] { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" };
        var companies = new[] { "Banco", "Amazon", "Netflix", "Apple", "Microsoft", "PayPal", "Google", "LinkedIn", "Instagram", "Facebook" };
        var names = new[] { "Carlos", "Ana", "Pedro", "Mariana", "João", "Fernanda", "Ricardo", "Juliana", "Lucas", "Patricia" };

        // Gerar 40 emails Fáceis (Easy)
        for (int i = 0; i < 40; i++)
        {
            bool isPhishing = i % 2 == 0; // Alterna entre phishing e legítimo
            string company = companies[random.Next(companies.Length)];
            
            emails.Add(new Email
            {
                SenderName = isPhishing ? $"{company} Security" : $"{company} Support",
                SenderEmail = isPhishing 
                    ? $"security@{company.ToLower()}-support.com" 
                    : $"no-reply@{company.ToLower()}.com",
                Subject = isPhishing
                    ? GetPhishingSubject(company, "Easy")
                    : GetLegitSubject(company, "Easy"),
                Body = isPhishing
                    ? GetPhishingBody(company, "Easy")
                    : GetLegitBody(company, "Easy"),
                Difficulty = "Easy",
                IsPhishing = isPhishing,
                Explanation = isPhishing
                    ? $"Phishing fácil: {GetPhishingExplanation(company, "Easy")}"
                    : $"E-mail legítimo: {GetLegitExplanation(company, "Easy")}"
            });
        }

        // Gerar 40 emails Médios (Medium)
        for (int i = 0; i < 40; i++)
        {
            bool isPhishing = i % 2 == 0;
            string company = companies[random.Next(companies.Length)];
            string name = names[random.Next(names.Length)];
            
            emails.Add(new Email
            {
                SenderName = isPhishing 
                    ? $"{name} {company}" 
                    : $"{company} Notifications",
                SenderEmail = isPhishing
                    ? $"{name.ToLower()}.{random.Next(100)}@{company.ToLower()}-access.com"
                    : $"notifications@{company.ToLower()}.com",
                Subject = isPhishing
                    ? GetPhishingSubject(company, "Medium")
                    : GetLegitSubject(company, "Medium"),
                Body = isPhishing
                    ? GetPhishingBody(company, "Medium")
                    : GetLegitBody(company, "Medium"),
                Difficulty = "Medium",
                IsPhishing = isPhishing,
                Explanation = isPhishing
                    ? $"Phishing médio: {GetPhishingExplanation(company, "Medium")}"
                    : $"E-mail legítimo: {GetLegitExplanation(company, "Medium")}"
            });
        }

        // Gerar 40 emails Difíceis (Hard)
        for (int i = 0; i < 40; i++)
        {
            bool isPhishing = i % 2 == 0;
            string company = companies[random.Next(companies.Length)];
            string name = names[random.Next(names.Length)];
            string domain = domains[random.Next(domains.Length)];
            
            emails.Add(new Email
            {
                SenderName = isPhishing
                    ? "Department of Security"
                    : $"{company} Accounts",
                SenderEmail = isPhishing
                    ? $"security.{random.Next(1000)}@{domain}"
                    : $"accounts@{company.ToLower()}.com",
                Subject = isPhishing
                    ? GetPhishingSubject(company, "Hard")
                    : GetLegitSubject(company, "Hard"),
                Body = isPhishing
                    ? GetPhishingBody(company, "Hard")
                    : GetLegitBody(company, "Hard"),
                Difficulty = "Hard",
                IsPhishing = isPhishing,
                Explanation = isPhishing
                    ? $"Phishing difícil: {GetPhishingExplanation(company, "Hard")}"
                    : $"E-mail legítimo: {GetLegitExplanation(company, "Hard")}"
            });
        }

        context.Emails.AddRange(emails);
        context.SaveChanges();
    }

    private static string GetPhishingSubject(string company, string difficulty)
    {
        var phrases = new[]
        {
            "URGENTE: Ação necessária na sua conta",
            "Atualização de segurança obrigatória",
            "Alerta: Atividade suspeita detectada",
            "Sua conta será bloqueada em 24 horas",
            "Verificação de conta pendente",
            "Acesso não autorizado detectado",
            "Confirmação de pagamento necessária",
            "Problema com seu último login",
            "Ação imediata: Conta comprometida",
            "Parabéns! Você ganhou um prêmio"
        };
        
        return difficulty switch
        {
            "Easy" => $"[{company}] {phrases[new Random().Next(phrases.Length)]}",
            "Medium" => $"{phrases[new Random().Next(phrases.Length)]} - {company}",
            "Hard" => $"Importante: {phrases[new Random().Next(phrases.Length)]}",
            _ => phrases[new Random().Next(phrases.Length)]
        };
    }

    private static string GetLegitSubject(string company, string difficulty)
    {
        var phrases = new[]
        {
            "Confirmação do seu pedido",
            "Atualização de conta",
            "Resumo mensal da sua conta",
            "Recebemos seu pagamento",
            "Seu recibo está disponível",
            "Novos recursos disponíveis",
            "Atualização de política de privacidade",
            "Convite para evento exclusivo",
            "Seu extrato está pronto",
            "Confirmação de cadastro"
        };
        
        return difficulty switch
        {
            "Easy" => $"[{company}] {phrases[new Random().Next(phrases.Length)]}",
            "Medium" => $"{phrases[new Random().Next(phrases.Length)]} - {company}",
            "Hard" => $"{company}: {phrases[new Random().Next(phrases.Length)]}",
            _ => phrases[new Random().Next(phrases.Length)]
        };
    }

    private static string GetPhishingBody(string company, string difficulty)
    {
        var domains = new[] { "secure-update.com", "account-verification.net", "access-security.org", "login-confirm.xyz" };
        var domain = domains[new Random().Next(domains.Length)];
        
        return difficulty switch
        {
            "Easy" => $@"Prezado cliente,

Detectamos atividade incomum em sua conta {company}. Para evitar o bloqueio permanente, clique no link abaixo para verificar suas informações:

http://{domain}/{company.ToLower()}-login

Este é um aviso automático - não responda este e-mail.

Atenciosamente,
Equipe de Segurança {company}",
            
            "Medium" => $@"Olá,

Sua conta {company} precisa de atualização imediata devido a novas medidas de segurança. 

Por favor, acesse nosso portal para confirmar seus dados:
http://{domain}/update-info?ref={new Random().Next(10000)}

Atenção: Contas não verificadas serão suspensas em 48 horas.

Equipe de Suporte {company}",
            
            "Hard" => $@"Caro usuário,

O departamento de segurança identificou acesso não autorizado ao sistema {company}. 

Para proteger seus dados, precisamos que confirme sua identidade:
https://{domain}/security-check?id={new Random().Next(100000)}

Nota: Esta é uma medida preventiva obrigatória para todos os clientes.

Atenciosamente,
Departamento de TI {company}",
            
            _ => "Corpo do e-mail"
        };
    }

    private static string GetLegitBody(string company, string difficulty)
    {
        return difficulty switch
        {
            "Easy" => $@"Olá,

Seu pedido na {company} foi processado com sucesso. Número de confirmação: #{new Random().Next(10000)}

Você pode acompanhar seu pedido em:
https://www.{company.ToLower()}.com/orders

Atenciosamente,
Equipe {company}",
            
            "Medium" => $@"Prezado cliente,

Atualizamos nossos termos de serviço. As principais mudanças incluem:

- Novas opções de privacidade
- Política de reembolso atualizada
- Melhorias na segurança

Leia os detalhes completos:
https://www.{company.ToLower()}.com/terms-update

Obrigado por usar {company}!",
            
            "Hard" => $@"Caro usuário,

Identificamos um novo login em sua conta {company}:

Data: {DateTime.Now.AddDays(-1):dd/MM/yyyy}
Localização: São Paulo, BR
Dispositivo: Chrome em Windows

Se foi você, ignore este e-mail. Caso contrário, proteja sua conta:
https://www.{company.ToLower()}.com/security

Atenciosamente,
Equipe de Segurança {company}",
            
            _ => "Corpo do e-mail legítimo"
        };
    }

    private static string GetPhishingExplanation(string company, string difficulty)
    {
        return difficulty switch
        {
            "Easy" => $"Domínio suspeito, URL não oficial e senso de urgência artificial",
            "Medium" => $"Domínio similar mas não oficial, linguagem de urgência e link suspeito",
            "Hard" => $"Contexto plausível mas com URL não oficial e solicitação incomum",
            _ => "Explicação de phishing"
        };
    }

    private static string GetLegitExplanation(string company, string difficulty)
    {
        return difficulty switch
        {
            "Easy" => $"Comunicação oficial sem solicitação de informações sensíveis",
            "Medium" => $"Notificação legítima com link para domínio oficial",
            "Hard" => $"Alerta de segurança real com instruções adequadas",
            _ => "Explicação legítima"
        };
    }
}