using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class GarageServices
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public Service Service { get; set; }

        public int GarageID { get; set; }
        public Garage Garage { get; set; }
    }
}