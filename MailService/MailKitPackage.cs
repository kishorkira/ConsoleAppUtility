using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace MailService
{
    public class MailKitPackage
    {
        public static void Send()
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("Name", "from@example.com"));
            mail.To.Add(new MailboxAddress("Name", "to@example.com"));
            mail.Subject = "Test Subject";
            mail.Body = new TextPart("plain") { Text = "Test Body" };
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.gmail.com", 587, false);

                    //update email provider settings (Less secure app access = on)
                    client.Authenticate("from@example.com", "password");

                    client.Send(mail);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            

        }

    }
}
