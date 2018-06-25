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
            var garages = _context.Garages.ToList();
            return View(garages);

        }
        public ActionResult Details(int id)
        {

            var garage = _context.Garages.
                SingleOrDefault(c => c.ID == id);
            if (garage == null)
                return HttpNotFound();
            else
                return View(garage);
        }
        public ActionResult New()
        {

            var garage = new Garage()
            {
                Name = "",
                Address = "",
                PhoneNumber = 0,
                ListOfServices = new List<Service>
                {
                    new Service
                    {
                        Title = "Oil Change",
                    Description="Some description, cost=70.00"
                    },
                    new Service
                    {
                        Title ="Suspension Check",Description="Some description, cost=100.00"
                    }
                }
            };

            return View("GarageForm", garage);
        }
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
                garageInDB.ListOfServices = garage.ListOfServices;


            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Garages");
        }
        public ActionResult Edit(int Id)
        {
            var customerInDB = _context.Customers.SingleOrDefault(c => c.ID == Id);

            if (customerInDB == null)
                return HttpNotFound();

            return View("CustomerForm", customerInDB);
        }

        //[Authorize(Roles = RoleNames.CanManageMedia)]
        public ActionResult Delete(int? id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.ID == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var customerInDB = _context.Customers.Find(id);

            _context.Customers.Remove(customerInDB);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}