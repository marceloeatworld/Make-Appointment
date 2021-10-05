using System;
using System.Collections.Generic;
using System.Text;

namespace rdvperso.Models
{
    public class NewContact
    {

        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public bool overwriteWithBlank { get; set; }

    }
}
