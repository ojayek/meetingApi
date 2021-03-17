using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using System.Xml;
using CreateXmlWebApi.Models;

namespace CreateXmlWebApi.Controllers
{
    public class EmailController : ApiController
    {
        [HttpGet]
        [Route("api/Email")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //[HttpGet]
        //[Route("api/Email/SendEmail")]

      public  void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }

        public Object SendEmail(Message model , XmlDocument doc)
        {

            using (MailMessage mm = new MailMessage(model.Email, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                mm.Bcc.Add(new MailAddress("ece@moshanir.co"));

                MemoryStream xmlStream = new MemoryStream();
                doc.Save(xmlStream);
                xmlStream.Flush();//Adjust this if you want read your data
                xmlStream.Position = 0;
                string fileName = createFileName();
                mm.Attachments.Add(new System.Net.Mail.Attachment(xmlStream, fileName));               
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "webmail.moshanir.co";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 25;
                mm.Headers.Add("X-ECE_send", "1.01");
                NEVER_EAT_POISON_Disable_CertificateValidation();
                smtp.Send(mm);



            }

            return model;
        }

        [HttpPost]
        [Route("api/Email/CreateXml")]
        public Object CreateXml()
        {
            try
            {
                var model = new Letter();
                var message = new Message();
                var httpContext = HttpContext.Current;
                HttpPostedFile httpPostedFile = null;
                // Check for any uploaded file  
                if (httpContext.Request.Files.Count > 0)
                {
                    httpPostedFile = httpContext.Request.Files[0];

                }

                var Code = Base64Decode(httpContext.Request.Headers["code"]);
                var Position = Base64Decode(httpContext.Request.Headers["position"]);
                var Department = Base64Decode(httpContext.Request.Headers["department"]);
                var LetterNo = Base64Decode(httpContext.Request.Headers["letterno"]);
                var Subject = Base64Decode(httpContext.Request.Headers["subject"]);
                var Organization = Base64Decode(httpContext.Request.Headers["organization"]);
                var To = httpContext.Request.Headers["to"];
                var Body = Base64Decode(httpContext.Request.Headers["body"]);
                model.Code = Code;
                model.Position = Position;
                model.Department = Department;
                model.LetterNo = LetterNo;
                model.Subject = Subject;
                model.Organization = Organization;

                message.To = To;
                message.Body = Body;
                message.Subject = Subject;
                //
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);

                //doc.LoadXml("<Letter><Protocol>wrench</Protocol></Letter>");
                XmlElement root = doc.CreateElement("Letter");
                XmlElement id = doc.CreateElement("Protocol");
                root.AppendChild(id);
                doc.AppendChild(root);
                id.SetAttribute("NAME", "ECE");
                id.SetAttribute("version", "1.0.1");
                XmlElement id2 = doc.CreateElement("Software");
                id2.SetAttribute("GUID", "{0200414A-0500-6F46-726D-73000053568B}");
                id2.SetAttribute("SoftwareDeveloper", "MiiroFiler");
                id2.SetAttribute("Version", "1");
                XmlElement id3 = doc.CreateElement("Sender");
                id3.SetAttribute("Code", "15560000");
                id3.SetAttribute("Department", "شرکت سهامی خدمات  مهندسی مشانیر");
                id3.SetAttribute("Name", "مدیر سیستم System Manager");
                id3.SetAttribute("Organization", "شرکت سهامی خدمات  مهندسی مشانیر");
                id3.SetAttribute("Position", "مدیر  سیستم");
                XmlElement id4 = doc.CreateElement("Receiver");
                id4.SetAttribute("Code", model.Code);
                id4.SetAttribute("Department", model.Department);
                id4.SetAttribute("Name", "");
                id4.SetAttribute("Organization", model.Organization);
                id4.SetAttribute("Position", model.Position);
                id4.SetAttribute("ReceiveType", "Origin");
                XmlElement id5 = doc.CreateElement("OtherReceivers");
                XmlElement otrs = doc.CreateElement("OtherReceiver");
                otrs.SetAttribute("Code", "");
                otrs.SetAttribute("Department", "");
                otrs.SetAttribute("Name", "");
                otrs.SetAttribute("Organization", "");
                otrs.SetAttribute("Position", "");
                otrs.SetAttribute("ReceiveType", "");
                XmlElement id6 = doc.CreateElement("LetterNo");
                id6.InnerText = model.LetterNo;
                XmlElement id7 = doc.CreateElement("LetterDateTime");
                id7.SetAttribute("ShowAs", "gregorian");
                id7.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                XmlElement id8 = doc.CreateElement("RelatedLetters");
                XmlElement id9 = doc.CreateElement("Subject");
                id9.InnerText = model.Subject;
                XmlElement id10 = doc.CreateElement("Priority");
                id10.SetAttribute("Code", "1");
                id10.SetAttribute("Name", "عادی");
                XmlElement id11 = doc.CreateElement("Classification");
                id11.SetAttribute("Code", "1");
                id11.SetAttribute("Name", "");
                //string imagePath = @"E:\sample.jpg";
                /// string imagePath = yourfile;
                string imgBase64String = "";
                string exttension = "";
                if (httpPostedFile != null)
                {
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(httpPostedFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(httpPostedFile.ContentLength);
                    }
                    imgBase64String = Convert.ToBase64String(fileData);
                    exttension = System.IO.Path.GetExtension(httpPostedFile.FileName);

                }

                XmlElement id12_1 = doc.CreateElement("Origins");
                XmlElement id12 = doc.CreateElement("Origin");
                id12.SetAttribute("ContentType", httpPostedFile.ContentType);
                id12.SetAttribute("Extension", exttension);
                id12.SetAttribute("Description", "با امضای مشانیر");
                id12.InnerText = imgBase64String;


                root.AppendChild(id2);
                root.AppendChild(id3);
                root.AppendChild(id4);
                root.AppendChild(id5);
                id5.AppendChild(otrs);
                root.AppendChild(id6);
                root.AppendChild(id7);
                root.AppendChild(id8);
                root.AppendChild(id9);
                root.AppendChild(id10);
                root.AppendChild(id11);
                id12_1.AppendChild(id12);
                root.AppendChild(id12_1);





                // Add a price element.
                // >>>>  XmlElement newElem = doc.CreateElement("Receiver");
                // >>>>>  newElem.InnerText = TextBox1.Text;
                //  doc.DocumentElement.AppendChild(newElem);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;


                SendEmail(message, doc);
                return doc;
            }
            catch (Exception ex)
            {

                return ex;
            }
         
        }

        public  string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public  string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string createFileName()
        {
          
            Random rnd = new Random();
            int str = rnd.Next(1000, 100000);
            return str.ToString() +".xml";
        }
    }
}
