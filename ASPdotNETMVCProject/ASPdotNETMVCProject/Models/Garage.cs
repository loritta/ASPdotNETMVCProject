using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Garage
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter the Garage's name.")]
        [StringLength(200)]
        [Display(Name = "Garage Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the Garage's address.")]
        [StringLength(200)]
        [Display(Name = "Garage Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter the Garage's phone.")]
       
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }

        public ICollection<GarageServices> GarageServices { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}