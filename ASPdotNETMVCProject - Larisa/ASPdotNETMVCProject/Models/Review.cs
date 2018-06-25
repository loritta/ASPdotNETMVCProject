using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class Review
    {
        public int ID { get; set; }
        public string TextReview { get; set; }
        public int Rating { get; set; }
    }
}