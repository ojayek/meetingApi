using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        private string _email;
        public string Email
        {
            get
            {
                return string.IsNullOrEmpty(_email) ? "ece@moshanir.co" : _email;
            }

            set { _email = value; }
        }
        private string _password;
        public string Password
        {
            get
            {
                return string.IsNullOrEmpty(_password) ? "ece@123" : _password;
            }

            set { _password = value; }
        }







    }
}