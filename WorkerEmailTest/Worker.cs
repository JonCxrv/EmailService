using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerEmailTest
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
                    var emails = new StreamReader("C:\\emailTeste\\emails.txt");
                    mail.From = new MailAddress("email");

                    foreach (var email in emails.ReadLine().Split(','))
                    {
                        mail.To.Add(email);
                    }
                    mail.Subject = "Bot Email test";
                    mail.IsBodyHtml = true;

                    var emailbody = File.ReadAllText("C:\\emailTeste\\email.html");
                    emailbody = emailbody.Replace("@Teste", "Vulgo dito");

                    mail.Body = emailbody;
                   
                        
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("email", "password");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                    _logger.LogInformation("Sen mail: {time}", DateTimeOffset.Now);

                }
                catch (Exception e)
                {

                    _logger.LogError("Error aplication: {error}",e);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
