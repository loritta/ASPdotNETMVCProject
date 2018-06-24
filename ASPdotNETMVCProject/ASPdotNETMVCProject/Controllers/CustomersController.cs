using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;

namespace ASPdotNETMVCProject.Controllers
{
    public class CustomersController : Controller
    {
        //DB Context Object
        private ApplicationDbContext _context;

        //class Constructor
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }
    }
}