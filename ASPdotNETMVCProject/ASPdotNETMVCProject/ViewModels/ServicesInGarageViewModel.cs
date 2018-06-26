using ASPdotNETMVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.ViewModels
{
    public class ServicesInGarageViewModel
    {
        public Garage Garage { get; set; }
        public IEnumerable<Service> Services { get; set; }
    }
}