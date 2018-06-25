using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public long PhoneNumber { get; set; }
        //public  AspNetUser AspNetUser { get; set; }
        //public byte Id { get; set; }

    }
}