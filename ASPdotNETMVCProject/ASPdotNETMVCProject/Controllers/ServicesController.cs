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
        [AllowAnonymous]
        public ActionResult Index(string SearchString, string sort)
        {
            string view = "ReadOnlyIndex";
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Index";
            }
            var services = _context.Services.ToList();
            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "name_desc" : "";
            
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                
                var searchTerms = SearchString.ToLower().Split(null);

                foreach (var term in searchTerms)
                {
                    string tmpTerm = term;
                    services = services.Where(s => s.Title.ToLower().Contains(SearchString) || 
                    s.Description.ToLower().Contains(SearchString)).ToList();
                  
                }         
                ViewBag.search = SearchString;
            }

            switch (sort)
            {
                case "name_desc":
                    services = services.OrderByDescending(c => c.Title).ToList();
                    break;
                default:
                    services = services.OrderBy(c => c.Title).ToList();
                    break;
            }

            return View(view, services);
            
        }
        public ActionResult Details(int id)
        {
            string view = "ReadOnlyDetails";
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Details";
            }
            var services = _context.Services.
                SingleOrDefault(c => c.ID == id);
            if (services == null)
                return HttpNotFound();
            else
                return View(view, services);
        }
    }
}