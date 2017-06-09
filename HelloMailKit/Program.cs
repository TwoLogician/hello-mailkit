using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace HelloMailKit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("From Name: ");
            var fromName = Console.ReadLine();
            Console.Write("From Address: ");
            var fromAddress = Console.ReadLine();
            Console.Write("To Name: ");
            var ToName = Console.ReadLine();
            Console.Write("To Address: ");
            var ToAddress = Console.ReadLine();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(new MailboxAddress(ToName, ToAddress));
            message.Subject = "Subject";

            message.Body = new TextPart("plain")
            {
                Text = "Body"
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                Console.Write("Host: ");
                var host = Console.ReadLine();
                Console.Write("Port: ");
                int.TryParse(Console.ReadLine(), out int port);
                Console.Write("SSL (y/n): ");
                var ssl = Console.ReadKey().Key == ConsoleKey.Y;
                client.Connect(host, port, ssl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                Console.WriteLine();
                Console.Write("Username: ");
                var username = Console.ReadLine();
                Console.Write("Password: ");
                var password = Console.ReadLine();
                client.Authenticate(username, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}