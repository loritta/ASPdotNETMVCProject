using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter the Customer's first name.")]
        [StringLength(200)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Customer's last name.")]
        [StringLength(200)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter the Customer's address.")]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter the Customer's phone number.")]
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }
        //public  AspNetUser AspNetUser { get; set; }
        //public byte Id { get; set; }

    }
}