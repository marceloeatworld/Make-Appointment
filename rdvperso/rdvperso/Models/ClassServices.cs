using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using rdvperso.Services;

namespace rdvperso.Models
{
    public class ClassServices 
    {   
        public int id { get; set; }
        public string name { get; set; }
        public int duration { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string availabilitiesType { get; set; }
        public int attendantsNumber { get; set; }
        public int categoryId { get; set; }
        public string FullImageUrl => description;
    }
}
