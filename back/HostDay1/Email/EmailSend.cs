
using hostDay.Models;
using hostDay.Planilhas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace hostDay.Email
{
    public class EmailSend
    { 
        public static  void enviarEmail(string para, string file)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add(para);
                mail.Subject = "Lista de cidades";
                mail.Body = "<h1>Obrigado por usar os serviços HostDay!</h1>";
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(file));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("hostdaynoreply@gmail.com", "hostacesso123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                }
 

            }


        }
        
        public static void threadEmails()
        {
            Task.Run(()=> {
                PlanilhaFactory factory = new PlanilhaFactory();
                while (true)
                { 
                    Task.Delay(360000); /// 1 minuto de atraso;
                   
                        List<Planilha> notSend = factory.PlanilhasNotSend();
                        foreach (Planilha toSend in notSend)
                        {
                            List<Cidade> planilha = factory.getPlanilha(toSend.id);
                            string file = factory.getPlanilha(planilha);
                            enviarEmail(toSend.email, file);
                            factory.deletePlanilha(toSend.id);
                            System.IO.File.Delete(file);
                        }
                    
                   

                }
            
            
            });
        }
    }

}
