using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;

namespace ASPdotNETMVCProject.Controllers
{
    public class ServicesController : Controller
    {
        //DB Context Object
        private ApplicationDbContext _context;

        //class Constructor
        public ServicesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Services
        public ActionResult Index()
        {
            var service = new Service();
            return View(service);
        }
    }
}