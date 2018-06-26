using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;
using ASPdotNETMVCProject.ViewModels;

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

        [AllowAnonymous]
        // GET: Garages
        public ActionResult Index(string SearchString, string sort)
        {
            string view = "ReadOnlyIndex";
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Index";
            }
            var garages = _context.Garages.ToList();

            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewBag.SortbyAddress = sort == "address" ? "address_desc" : "address";

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                /* LINQ Code
                customers = (from c in customers
                             where c.Name.Contains(SearchString)
                             select c);*/
                garages = garages.Where(c => c.Name.Contains(SearchString)).ToList();
                ViewBag.search = SearchString;
            }

            switch (sort)
            {
                case "name_desc":
                    garages = garages.OrderByDescending(c => c.Name).ToList();
                    break;
                case "adress":
                    garages = garages.OrderBy(c => c.Address).ToList();
                    break;
                case "adress_desc":
                    garages = garages.OrderByDescending(c => c.Address).ToList();
                    break;
                default:
                    garages = garages.OrderBy(c => c.Name).ToList();
                    break;
            }
            return View(view,garages);

        }
        [AllowAnonymous]
        public ActionResult Details(int id)
        {

            var garage = _context.Garages.
                SingleOrDefault(c => c.ID == id);
            if (garage == null)
                return HttpNotFound();
            else
                return View(garage);
        }
        public ActionResult ListOfServices(int id)
        {
            var viewModel = new ServicesInGarageViewModel()
            {
                Garage = _context.Garages.SingleOrDefault(c => c.ID == id),
                Services = (from data in _context.Garages
                            join data2 in _context.GarageServices on data.ID equals data2.GarageID
                            join data3 in _context.Services on data2.ServiceID equals data3.ID
                            where data.ID == id
                            select data3).ToList()

            };

            return View(viewModel);

        }
        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        public ActionResult New()
        {

            var viewModel = new ServicesInGarageViewModel()
            {
                Garage = new Garage(),
                Services = (from data in _context.Garages
                            join data2 in _context.GarageServices on data.ID equals data2.GarageID
                            join data3 in _context.Services on data2.ServiceID equals data3.ID
                            select data3).ToList()

            };

            return View("GarageForm", viewModel);
        }
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Garage garage)
        {

            if (!ModelState.IsValid)
            {
                //The form is not valid --> return same form to the user


                return View("CustomerForm", garage);
            }
            //******** Come here if form is valid
            if (garage.ID == 0)
            {
                _context.Garages.Add(garage);
            }
            else
            {
                var garageInDB = _context.Garages.Single(g => g.ID == garage.ID);



                //Manually update the fields I want.
                garageInDB.Name = garage.Name;
                garageInDB.Address = garage.Address;
                garageInDB.PhoneNumber = garage.PhoneNumber;
                


            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Garages");
        }

        [Authorize(Roles = RoleNames.Administrator)]
        public ActionResult Edit(int Id)
        {
            var garageInDB = _context.Garages.SingleOrDefault(c => c.ID == Id);

            if (garageInDB == null)
                return HttpNotFound();

            return View("GarageForm", garageInDB);
        }

        [Authorize(Roles = RoleNames.Administrator)]
        //[Authorize(Roles = RoleNames.CanManageMedia)]
        public ActionResult Delete(int? id)
        {
            var garage = _context.Garages.SingleOrDefault(c => c.ID == id);

            if (garage == null)
                return HttpNotFound();

            return View(garage);
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var garageInDB = _context.Garages.Find(id);

            _context.Garages.Remove(garageInDB);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}