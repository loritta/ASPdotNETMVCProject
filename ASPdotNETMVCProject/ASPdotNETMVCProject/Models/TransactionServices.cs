using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class TransactionServices
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public Service Service { get; set; }

        public int TransactionID { get; set; }
        public Transaction Transaction { get; set; }
    }
}