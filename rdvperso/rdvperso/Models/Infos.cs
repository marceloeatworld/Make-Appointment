using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace rdvperso.Models
{
    [Table("Infos")]
    public class Infos 
    {
    

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string notes { get; set; }






    }
}
