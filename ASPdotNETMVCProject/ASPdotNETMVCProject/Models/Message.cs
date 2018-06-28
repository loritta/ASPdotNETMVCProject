using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Message
    {
        public int ID { get; set; }

        public Customer Customer { get; set; }
        public int CustomerID { get; set; }

        public Garage Garage { get; set; }
        public int GarageID { get; set; }

        public string Title { get; set; }
        public string Contents { get; set; }
        /*1
         * 2 message link
         */


    }
}