using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.IO;

namespace Service.Helpers
{
    public class EmailHelper
    {
        public static bool IsEmail(string email)
        {
            string MatchEmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int SMTPSendEmail(int port, string host, int timeout, string user, string password, string emailFrom, List<string> emailTo, List<string> emailCC, string subject, string filename, string body)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = port;
            smtpClient.Host = host;

            smtpClient.EnableSsl = true;
            smtpClient.Timeout = timeout;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(user, password);


            MailMessage mail = new MailMessage();
            try
            {
                foreach (string item in emailTo)
                {
                    if (EmailHelper.IsEmail(item.Trim()))
                    {
                        mail.To.Add(item.Trim());
                    }
                }
                foreach (string item in emailCC)
                {
                    if (EmailHelper.IsEmail(item.Trim()))
                    {
                        mail.CC.Add(item.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(filename))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);
                    Stream stream = new MemoryStream(File.ReadAllBytes(filename));
                    mail.Attachments.Add(new System.Net.Mail.Attachment(stream, fileInfo.Name, null));
                }
                mail.From = new MailAddress(emailFrom);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;

                smtpClient.Send(mail);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}
