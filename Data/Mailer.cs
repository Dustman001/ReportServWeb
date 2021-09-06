using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    class Mailer
    {        
        private EmailMessage _em;
        List<string[]> _attach = new List<string[]>();

        private readonly string _user = "ivpadron@edenorte.com.do";
        private readonly string _pass = "Mc123456";

        public Mailer()
        {

        }

        public void setMailer(MailMessage mm, List<string[]> matt)
        {
            _attach = matt;

            _em = Mailer3();

            foreach (MailAddress ma in mm.To)
            {
                _em.ToRecipients.Add(ma.DisplayName, ma.Address);
            }

            foreach (MailAddress ma in mm.CC)
            {
                _em.CcRecipients.Add(ma.DisplayName, ma.Address);
            }

            foreach (MailAddress ma in mm.Bcc)
            {
                _em.BccRecipients.Add(ma.DisplayName, ma.Address);
            }

            _em.Subject = mm.Subject;
            _em.Body = mm.Body;
        }

        public bool SendExchange()
        {
            foreach (string[] at in _attach)
            {
                _em.Attachments.AddFileAttachment(at[0], at[1]);
            }

            try
            {
                _em.Send();
            }
            catch (Exception x)
            {
                _em.Attachments.Clear();
                return false;
            }

            return true;
        }

        private EmailMessage Mailer3()
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

            service.Credentials = new WebCredentials(_user, _pass, "Edenorte");//("jtcornorte@edenorte.com.do", "jtcornorte", "Dell-2020*");

            service.AutodiscoverUrl(_user, RedirectionUrlValidationCallback);
            service.Timeout = 300000;

            return new EmailMessage(service);
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);

            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }
    }
}
