using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Portfolio.Custom
{
    /// <summary>
    /// Summary description for SendEmail
    /// </summary>
    public class SendEmail : IHttpHandler
    {
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactSubject { get; set; }
        public string ContactMessage { get; set; }
        public void ProcessRequest(HttpContext context)
        {//reading QS and intializing the message properties
            context.Response.ContentType = "text/plain";
            if (context.Request.RequestType == "POST")
            {
                try
                {
                    ReadQuesryString(context);
                    //sending the mail
                    SendMail();
                    context.Response.Write("OK");
                }
                catch (Exception e)
                {
                    context.Response.Write(e.Message);
                }
            }
            else
            {
                //not allowed
                context.Response.Write("Not allowed");
            }

        }

        private void SendMail()
        {
            var client = new SmtpClient()
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EncryptionHelper.Decrypt(ConfigurationManager.AppSettings["EmailMailer"]), EncryptionHelper.Decrypt(ConfigurationManager.AppSettings["EmailMailerPwd"]))
            };
            var bodyText = string.Format("You received a message from {0}\n\n----------------------------------------------------------------------\n\n{1}", ContactName, ContactMessage);
            var mail = new MailMessage(ContactEmail, EncryptionHelper.Decrypt(ConfigurationManager.AppSettings["EmailTo"]), ContactSubject, bodyText)
            {
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };
            //send a carbon copy to the sender
            mail.CC.Add(ContactEmail);
            client.Send(mail);
        }

        private void ReadQuesryString(HttpContext context)
        {
            ContactName = context.Request["contactName"];
            ContactEmail = context.Request["contactEmail"];
            ContactSubject = context.Request["contactSubject"];
            ContactMessage = context.Request["contactMessage"];
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}