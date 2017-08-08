using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Configuration;

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
            //if (context.Request.RequestType == "POST")
            //{
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
            //}
            //else
            //{
            //    //not allowed
            //    context.Response.Write("Not allowed");
            //}

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
                Credentials = new System.Net.NetworkCredential(EncryptionHelper.Decrypt(ConfigurationManager.AppSettings["EmailMailer"]), ConfigurationManager.AppSettings["EmailMailerPwd"])
            };
            var mail = new MailMessage(ContactEmail, EncryptionHelper.Decrypt(ConfigurationManager.AppSettings["EmailTo"]), ContactSubject, ContactMessage)
            {
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };
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