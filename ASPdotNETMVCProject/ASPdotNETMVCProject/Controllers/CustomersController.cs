using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;
using Microsoft.AspNet.Identity;

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
        public ActionResult Index(string SearchString, string sort)
        {
            //Check for user
            string view = "ReadOnlyIndex";
            if (User.IsInRole(RoleNames.Administrator)|| User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Index";
            }
            var customers = _context.Customers.ToList();
            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "first_name_desc" : "";
            ViewBag.SortbyAddress = sort == "last_name" ? "last_name_desc" : "last_name";

             if (!string.IsNullOrWhiteSpace(SearchString))
            {
                /* LINQ Code
                customers = (from c in customers
                             where c.Name.Contains(SearchString)
                             select c);*/
                customers =customers.Where(c => c.FirstName.Contains(SearchString)).ToList();
                ViewBag.search = SearchString;
            }

            switch (sort)
            {
                case "first_name_desc":
                    customers = customers.OrderByDescending(c => c.FirstName).ToList();
                    break;
                case "last_name":
                    customers = customers.OrderBy(c => c.LastName).ToList();
                    break;
                case "last_name_desc":
                    customers = customers.OrderByDescending(c => c.LastName).ToList();
                    break;
                default:
                    customers = customers.OrderBy(c => c.FirstName).ToList();
                    break;
            }

            return View(view,customers);
        }
        public ActionResult Details(int id)
        {
            string view = "ReadOnlyDetails";
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Details";
            }
            var customer = _context.Customers.               
                SingleOrDefault(c => c.ID == id);
            if (customer == null)
                return HttpNotFound();
            else
                return View(view,customer);
        }
        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        public ActionResult New()
        {
           var customer = new Customer();
           
            return View("CustomerForm", customer);

        }

        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
           /* var currentUserId = User.Identity.GetUserId();
            var customerInfo = _context.Customers.FirstOrDefault(d => d.ID = currentUserId);
            if (customerInfo == null)
            {
                if (pasinfo == null)
                {
                    pasinfo = db.pas.Create();
                    pasinfo.UserId = currentUserId;
                    db.pas.Add(pasinfo);
                }

                pasinfo.FirstName = p.FirstName;
                db.SaveChanges();
            }*/  
                if (!ModelState.IsValid)
            {
                //The form is not valid --> return same form to the user
                

                return View("CustomerForm", customer);
            }
            //******** Come here if form is valid
            if (customer.ID == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDB = _context.Customers.Single(c => c.ID == customer.ID);

               

                //Manually update the fields I want.
                customerInDB.FirstName = customer.FirstName;
                customerInDB.LastName = customer.LastName;
                customerInDB.Address = customer.Address;
                customerInDB.PhoneNumber = customer.PhoneNumber;
               

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        public ActionResult Edit(int Id)
        {
            var customerInDB = _context.Customers.SingleOrDefault(c => c.ID == Id);

            if (customerInDB == null)
                return HttpNotFound();

            return View("CustomerForm", customerInDB);
        }

        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        //[Authorize(Roles = RoleNames.CanManageMedia)]
        public ActionResult Delete(int? id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.ID == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
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