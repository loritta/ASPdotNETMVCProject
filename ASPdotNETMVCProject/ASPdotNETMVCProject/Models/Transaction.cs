using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Transaction
    {
        public int ID { get; set; }

        public List<Service> Services { get; set; }
        public int ServiceID { get; set; }

        public Customer Customer { get; set; }
        public int CustomerID { get; set; }

        public Garage Garage { get; set; }
        public int GarageID { get; set; }
    }
}