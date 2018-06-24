using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;

namespace ASPdotNETMVCProject.Controllers
{
    public class GaragesController : Controller
    {
        //DB Context Object
        private ApplicationDbContext _context;

        //class Constructor
        public GaragesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Garages
        public ActionResult Index()
        {
           
            return View();
           
        }
    }
}