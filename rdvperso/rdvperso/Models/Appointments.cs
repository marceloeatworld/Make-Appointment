using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace rdvperso.Models
{
    public class Appointments
    {
        public int id { get; set; }
        public string book { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string hash { get; set; }
        public string notes { get; set; }
        public int customerId { get; set; }
        public int providerId { get; set; }
        public int serviceId { get; set; }
        public object googleCalendarId { get; set; }
    }
}
