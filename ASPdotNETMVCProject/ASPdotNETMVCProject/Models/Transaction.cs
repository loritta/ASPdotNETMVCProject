using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Transaction
    {
        public int ID { get; set; }

        public List<Service> ListOfServices { get; set; }


        //this has to come automatically based on the user login for the customer 
        //and as a dropdown for the garage


        [Required(ErrorMessage = "Please enter the Customer's name.")]
        [StringLength(200)]
        [Display(Name = "Customer's Name")]
        public Customer Customer { get; set; }
        public int CustomerID { get; set; }

        //this has to come automatically based on the garage logged in for the garage 
        //and as a dropdown for the customer

        [Required(ErrorMessage = "Please enter the Garage's name.")]
        [StringLength(200)]
        [Display(Name = "Garage's Name")]
        public Garage Garage { get; set; }
        public int GarageID { get; set; }
    }
}