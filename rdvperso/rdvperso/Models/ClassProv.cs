using System;
using System.Collections.Generic;
using System.Text;

namespace rdvperso.Models
{
    public class ClassProv
    {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string mobile { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string notes { get; set; }
            public string timezone { get; set; }
            public List<string> services { get; set; }
            public string FullImageUrl => notes;
    }
}
