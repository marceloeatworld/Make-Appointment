using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace rdvperso.Models
{
    public class Categories 
    {     
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string FullImageUrl => description;
    }
}
