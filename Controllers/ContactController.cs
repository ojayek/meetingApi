using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using CreateXmlWebApi.Models;

namespace CreateXmlWebApi.Controllers
{
    public class ContactController : ApiController
    {
        // GET: api/Contact

        private IntranetDatabaseString db = new IntranetDatabaseString();
        [HttpGet]
        [Route("api/Contact/GetContactList")]
        public IEnumerable<Person> GetContactList()
        {
            var data = db.View_ContactList.ToList();
            var lstContacts = new List<Person>();

            foreach (var item in data)
            {
                var prsdata = new Person();
                prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
                prsdata.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك','ک'):null;
                prsdata.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
                prsdata.Moavenat =(item.Moavenat !=null)? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.Email = item.Email;
                prsdata.Proj_Name = (item.Proj_Name != null) ? item.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NumBuild = (item.NumBuild != null) ? item.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NamLatin = item.NamLatin;
                prsdata.Sharh_Onvan = (item.Sharh_Onvan != null) ? item.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
                {
                    lstContacts.Add(prsdata);
                }
            }
            var teldatas = db.Contact.Where(o => (!string.IsNullOrEmpty(o.Tel) || !string.IsNullOrEmpty(o.DirectPhoneNo)));
            foreach (var item in teldatas)
            {
                var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
                if (contact != null)
                {
                    contact.Tel = item.Tel;
                    contact.DirectPhoneNo = item.DirectPhoneNo;
                }
            }
            return lstContacts;
        }

        [HttpGet]
        [Route("api/Contact/GetContactByPrsNum/{PrsNum}")]
        public IEnumerable<Person> GetContactByPrsNum(int prsnum)
        {
            var data = db.View_ContactList.Where(o=>o.Prsnum==prsnum).ToList();
            var lstContacts = new List<Person>();

            foreach (var item in data)
            {
                var prsdata = new Person();
                prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
                prsdata.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
                prsdata.Moavenat = (item.Moavenat != null) ? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.Email = item.Email;
                prsdata.Proj_Name = (item.Proj_Name != null) ? item.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NumBuild = (item.NumBuild != null) ? item.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.NamLatin = item.NamLatin;
                prsdata.Sharh_Onvan = (item.Sharh_Onvan != null) ? item.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
                {
                    lstContacts.Add(prsdata);
                }
            }
            var teldatas = db.Contact.Where(o => o.prsnum==prsnum);
            foreach (var item in teldatas)
            {
                var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
                if (contact != null)
                {
                    contact.Tel = item.Tel;
                    contact.DirectPhoneNo = item.DirectPhoneNo;
                }
            }
            return lstContacts;
        }

        [HttpPost]
        [Route("api/Contact/CreateContact")]
        public Contact CreateContact([FromBody] ContactData data)
        {
            var contact = new Contact();
            var insertdBefor= db.Contact.Where(o => o.prsnum == data.Prsnum).SingleOrDefault();
            var MainData = db.View_ContactList.Where(o => o.Prsnum == data.Prsnum).SingleOrDefault();

            if (insertdBefor != null)
            {
                insertdBefor.DirectPhoneNo = data.DirectPhoneNo;
                insertdBefor.Tel = data.Tel;
                insertdBefor.Name= MainData.Nam.Replace('ي', 'ی').Replace('ك', 'ک') + " " + MainData.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک');
                insertdBefor.OfficePosition = (MainData.Proj_Name != null) ? MainData.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                insertdBefor.Title = (MainData.Sharh_Onvan != null) ? MainData.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                insertdBefor.Building = (MainData.NumBuild != null) ? MainData.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                insertdBefor.Deputy = (MainData.Moavenat != null) ? MainData.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک'):null; 
                insertdBefor.Pic = MainData.Prsnum + ".jpg";
                insertdBefor.Enabled = true;
             
                db.SaveChanges();
                
                return insertdBefor;
            }
            else
            {
                contact.DirectPhoneNo = data.DirectPhoneNo;
                contact.Tel = data.Tel;
                contact.Name = MainData.Nam.Replace('ي', 'ی').Replace('ك', 'ک') + " " + MainData.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک');
                contact.OfficePosition = (MainData.Proj_Name != null) ? MainData.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                contact.Title =    (MainData.Sharh_Onvan != null) ? MainData.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                contact.Building = (MainData.NumBuild != null) ? MainData.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                contact.Deputy =   (MainData.Moavenat != null) ? MainData.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                contact.prsnum = MainData.Prsnum;
                contact.Pic = MainData.Prsnum + ".jpg";
                contact.Enabled = true;

                db.Contact.Add(contact);                
                db.SaveChanges();
                return contact;
            }                                 
        }

        [HttpGet]
        [Route("api/Contact/decode/{percode}")]
        public resToken decode(string percode)
        {
            string lbl1 = percode;
            resToken res = new resToken();
            try
            {
                string EncryptionKey1 = "MIS2015MIS";
                lbl1 = percode.Replace(" ", "+");
                byte[] cipherBytes1 = Convert.FromBase64String(lbl1);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey1, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes1, 0, cipherBytes1.Length);
                            cs.Close();
                        }
                        res.isValid = true;
                        res.PrsCode = Encoding.Unicode.GetString(ms.ToArray());
                        var prsData = GetContactByPrsNum(int.Parse(res.PrsCode));
                        res.Data = prsData.FirstOrDefault();
                        var prsCode = int.Parse(res.PrsCode);
                        var adminData= db.TelAdmin.Where(o => o.Prsnum == (prsCode) && o.Enabled == true).SingleOrDefault();
                        if (adminData != null)
                        {
                            res.isAdmin = true;
                        }
                        else res.isAdmin = false;
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {

                res.isValid = false;
                res.PrsCode = ex.Message;
                res.Data = null;
                return res;
            }
          
        }
    }

    public  class resToken
    {
        public string PrsCode { get; set; }
        public bool isValid { get; set; }
        public bool isAdmin { get; set; }
        public Person Data { get; set; }
    }
}
