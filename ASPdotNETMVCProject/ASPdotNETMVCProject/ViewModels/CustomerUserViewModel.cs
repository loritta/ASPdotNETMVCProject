using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPdotNETMVCProject.Models;

namespace ASPdotNETMVCProject.ViewModels
{
    public class CustomerUserViewModel
    {
        public Customer Customer { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}