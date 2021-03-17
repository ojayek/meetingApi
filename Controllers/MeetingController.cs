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
    public class MeetingController : ApiController
    {
        // GET: api/Contact

        private MoshanirMeetingEntities db = new MoshanirMeetingEntities();
        [HttpGet]
        [Route("api/Meeting/GetAllMeeting")]
        public IEnumerable<Meetings> GetAllMeeting()
        {
            var data = db.Meetings.ToList();
            var lstContacts = new List<Meetings>();

            foreach (var item in data)
            {
                var prsdata = new Meetings();
                //prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
                //prsdata.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك','ک'):null;
                //prsdata.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
                //prsdata.Moavenat =(item.Moavenat !=null)? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.Email = item.Email;
                //prsdata.Proj_Name = (item.Proj_Name != null) ? item.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.NumBuild = (item.NumBuild != null) ? item.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.NamLatin = item.NamLatin;
                //prsdata.Sharh_Onvan = (item.Sharh_Onvan != null) ? item.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
                //{
                //    lstContacts.Add(prsdata);
                //}
                lstContacts.Add(prsdata);
            }
            var teldatas = db.Meetings.Where(o => (!string.IsNullOrEmpty(o.Title) || !string.IsNullOrEmpty(o.Title)));
            //foreach (var item in teldatas)
            //{
            //    var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
            //    if (contact != null)
            //    {
            //        contact.Tel = item.Tel;
            //        contact.DirectPhoneNo = item.DirectPhoneNo;
            //    }
            //}
            return lstContacts;
        }

        [HttpGet]
        [Route("api/Meeting/GetMeetingById/{Id}")]
        public IEnumerable<Meetings> GetMeetingById(int Id)
        {
            var data = db.Meetings.Where(o=>o.Id==Id).ToList();
            var lstContacts = new List<Meetings>();

            foreach (var item in data)
            {
                var prsdata = new Meetings();
                prsdata.Id = int.Parse(item.Id.ToString());
                prsdata.Title = (item.Title != null) ? item.Title.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.InnerParticipator = (item.InnerParticipator != null) ? item.InnerParticipator.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                
                prsdata.OuterParticipator = (item.OuterParticipator != null) ? item.OuterParticipator.Replace('ي', 'ی').Replace('ك', 'ک'):null;
               
                prsdata.Location = (item.Location != null) ? item.Location.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                prsdata.MeetingDate = item.MeetingDate;
                if (!lstContacts.Where(o => o.Id == prsdata.Id).Any())
                {
                    lstContacts.Add(prsdata);
                }
            }
            
            
            return lstContacts;
        }

        [HttpPost]
        [Route("api/Meeting/CreateMeeting")]
        public Meetings CreateMeeting([FromBody] MeetingData data)
        {
            var newMeeting = new Meetings();
            newMeeting.Title = data.Title;
            newMeeting.MeetingNumber = data.MeetingNumber;
            newMeeting.InnerParticipator = data.InnerParticipators;
            newMeeting.OuterParticipator = data.OuterParticipators;
            newMeeting.Location = data.Location;
           // newMeeting.MeetingDate = data.MeetingDate;
            db.Meetings.Add(newMeeting);
            db.SaveChanges();

            var afterInsert = db.Meetings.Where(o => o.MeetingNumber == newMeeting.MeetingNumber).SingleOrDefault();
            if(afterInsert != null)
            {
                foreach (var item in data.lstSubjects)
                {
                    var newSubject = new MeetingSubjects();
                    newSubject.SubTitle = item.Subject;
                    newSubject.Responsible = item.tracingResponsible;
                  //  newSubject.DeadLine = item.endDate;
                    newSubject.MeetingId = afterInsert.Id;
                    db.MeetingSubjects.Add(newSubject);
                    db.SaveChanges();
                }
            }
            return afterInsert;
        }

     
    
    }

   
}
