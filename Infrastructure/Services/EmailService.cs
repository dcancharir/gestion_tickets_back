using Application.Ports.Driven;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Infrastructure.Services;

public class EmailService : IEmailService {
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration) {
        _configuration = configuration;
    }

    public async Task SendEmail(string to, string subject, string body, bool isHtml = false) {
		try {
            var _from = _configuration["Email:Correo"]!;
            var _password = _configuration["Email:Password"]!;
            using(var mail = new MailMessage()) {
                mail.From = new MailAddress(_from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isHtml;

                using(var smtp = new SmtpClient("smtp.gmail.com", 587)) {
                    smtp.Credentials = new NetworkCredential(_from, _password);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(mail);
                }
            }
        } catch(Exception) {

			throw;
		}
    }
}
