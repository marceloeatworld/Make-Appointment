using System;
using System.Collections.Generic;
using System.Text;
using rdvperso.Services;

namespace rdvperso.Models
{
    public class Aggregates
    {
        public class Provider
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
        }

        public class Customer
        {
            public int id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
            public object notes { get; set; }
        }


        public class Service
        {
            public int id { get; set; }
            public string name { get; set; }
            public int duration { get; set; }
            public double price { get; set; }
            public string currency { get; set; }
            public string description { get; set; }
            public string availabilitiesType { get; set; }
            public int attendantsNumber { get; set; }
            public int categoryId { get; set; }
            public string FullImageUrl => description;
        }



        public class MyArray
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
            public Provider provider { get; set; }
            public Customer customer { get; set; }
            public Service service { get; set; }
        }

        public class Root
        {
            public List<MyArray> MyArray { get; set; }
        }


    }
}