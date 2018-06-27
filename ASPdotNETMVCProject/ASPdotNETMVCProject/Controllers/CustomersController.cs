using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASPdotNETMVCProject.Controllers
{
    [Authorize]
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
            var customers = _context.Customers.ToList();
            //Check for user
            string view = "ReadOnlyIndex";
            if (User.IsInRole(RoleNames.Administrator))
            {
                view = "Index";
            }
            else if (User.IsInRole(RoleNames.Customer))
            {
                //customers = _context.Customers.Where(c=>c.ID ==)
                return View("CustomerForm");
            }
            else if (User.IsInRole(RoleNames.GarageOwner))
            {
                //we need to create a custom viewmodel
            }


           

            //saving the search to view
            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "first_name_desc" : "";
            ViewBag.SortbyAddress = sort == "last_name" ? "last_name_desc" : "last_name";

            //searching
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                /* LINQ Code
                customers = (from c in customers
                             where c.Name.Contains(SearchString)
                             select c);*/
                var searchTerms = SearchString.ToLower().Split(null);

                foreach (var term in searchTerms)
                {
                    string tmpTerm = term;
                    customers = customers.Where(c => c.FirstName.ToLower().Contains(SearchString) ||
                    c.LastName.ToLower().Contains(SearchString)).ToList();

                }
                ViewBag.search = SearchString;
            }

            //sorting the search
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

            return View(view, customers);
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
                return View(view, customer);
        }
        [Authorize]
        public ActionResult New()
        {
            var customer = new Customer();
            //Checking the role of the user

            //if it's an admin or a garage or a user who has the right to add customers
            if (User.IsInRole(RoleNames.Customer))
            {
                return View("AlreadyInRole");
            }
            else
            {
                return View("CustomerForm", customer);
            }

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {

            if (!ModelState.IsValid)
            {

                //The form is not valid --> return same form to the user


                return View("CustomerForm", customer);
            }

            //******** Come here if form is valid
            if (customer.ID == 0)
            {
                Security security = new Security();
                security.AddUserToRole(User.Identity.GetUserName(), RoleNames.Customer);
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
                Security security = new Security();
                security.AddUserToRole(User.Identity.GetUserName(), RoleNames.Customer);
            }

            _context.SaveChanges();
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                return RedirectToAction("Index", "Customers");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [Authorize]
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