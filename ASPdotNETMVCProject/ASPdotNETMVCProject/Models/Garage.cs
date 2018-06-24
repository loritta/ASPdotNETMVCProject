using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Garage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long PhoneNumber { get; set; }

        public List<Service>Services { get; set; }
        public int ServiceID { get; set; }

    }
}