
using System;      
using System.Collections.Generic;      
using System.Linq;      
using System.Net;      
using System.Net.Http;      
using System.Web.Http;      
using CreateXmlWebApi.Models;
namespace CreateXmlWebApi.Controllers
{ 


    [RoutePrefix("api/Meetings")]
    public class ShowMeetingController : ApiController
    {
        MoshanirMeetingModel DB = new MoshanirMeetingModel();

        [Route("Meetings")]
        [HttpGet]
        public object Meetings()
        {
            return DB.Meetings.ToList();
        }


    }
}