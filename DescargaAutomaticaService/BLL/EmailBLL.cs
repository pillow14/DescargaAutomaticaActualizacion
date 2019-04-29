using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace DescargaAutomaticaService.BLL
{
    public class EmailBLL
    {
        FileInfo[] files = null;
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        System.Net.Mail.Attachment attachment;
        Utilitario.Utiles utiles = new Utilitario.Utiles();

        /// <summary>
        /// Envio de archivos descargables comprimidos por correo electronico
        /// </summary>
        public void EnvioArchivoActualizacionSoftware()
        {
            DirectoryInfo d = new DirectoryInfo(ConfigurationManager.AppSettings["carpetaDescargar"].ToString());//Assuming Test is your Folder
            files = d.GetFiles("*.exe"); //Getting Text files
            int contadorArchivos = files.Count();

            if (contadorArchivos > 0)
            {
                var fromAddress = new MailAddress("victor.soto.droguett@gmail.com", "Test");
                var toAddress = new MailAddress("victor.soto.droguett@gmail.com", "Victor Soto");
                const string fromPassword = "tommyoddy15";
                const string subject = "Actualizacion Archivo Soporte- CEPET";
                const string body = "Se adjuntar archivos de actualización para descargar";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                //comprimir archivos y adjuntar
                foreach(var archivo in files)
                {
                    utiles.Compress(archivo);
                    var algo = d.GetFiles("*.gz");
                    foreach (var algodos in algo)
                    {
                        attachment = new System.Net.Mail.Attachment(algodos.FullName);
                    }

                }

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body                  
                    
                })
                {
                    message.Attachments.Add(attachment);
                    smtp.Send(message);
                }

            }
            else
            {
                mail.From = new MailAddress("victor.soto.droguett@gmail.com");
                mail.To.Add("victor.soto.droguett@gmail.com");
                mail.Subject = "Descarga de archivos automaticos- Actualización CEPET";
                mail.Body = "No se encontró ningún archivo para descargar.";

                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential("victor.soto.droguett@gmail.com", "tommyoddy15");
                    smtp.Timeout = 20000;
                }
                // Passing values to smtp object
                smtp.Send(mail);
            }
        }

        public void EnviarMailAdmin(string error, string nombreProceso)
        {

        }
    }
}
