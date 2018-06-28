using ASPdotNETMVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace ASPdotNETMVCProject.ViewModels
{
    public class MessageCustomerGaragesViewModel
    {
        public Models.Message Message { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Garage> Garages { get; set; }
    }
}