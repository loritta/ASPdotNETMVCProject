using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;
using ASPdotNETMVCProject.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASPdotNETMVCProject.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        //DB Context Object
        private ApplicationDbContext _context;
        private Security security;
        //class Constructor
        public CustomersController()
        {
            _context = new ApplicationDbContext();
            security = new Security();
        }
        // GET: Customers
        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        public ActionResult Index(string SearchString, string sort)
        {
            var customers = _context.Customers.ToList();
            //Check for user
            string view = "ReadOnlyIndexUser";
            if (User.IsInRole(RoleNames.Administrator))
            {
                view = "Index";
            }
            else if (User.IsInRole(RoleNames.Customer))
            {
                view = "CustomerForm";
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
        [Authorize(Roles = RoleNames.AdministratorGarageOwnerCustomer)]
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
            var customerToReturn = new Customer();
            var form = "CustomerFormUser";
            
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                form = "CustomerForm";
                 var viewModel = new CustomerUserViewModel()
                {
                    Customer = new Customer(),

                    ApplicationUser = _context.Users.SingleOrDefault(c => c.Id == customerToReturn.ApplicationUserId)
                };
                return View(form, viewModel);
            }
            else if (User.IsInRole(RoleNames.Customer))
            {
                form = "AlreadyInRole";
                return View(form);
            }
            else
            {
                return View(form);
            }
           
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                //The form is not valid --> return same form to the user


                return View("CustomerFormCustomer", customer);
            }
            //******** Come here if form is valid
           
                var customerInDB = _context.Customers.Single(g => g.ID == customer.ID);



            //Manually update the fields I want.
            customerInDB.FirstName = customer.FirstName;
            customerInDB.LastName = customer.LastName;
            customerInDB.Address = customer.Address;
            customerInDB.PhoneNumber = customer.PhoneNumber;

            Session["customerLoggedIn"] = customer;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
   
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            
            if (!ModelState.IsValid)
            {
                var form = "CustomerFormUser";
                if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner) || User.IsInRole(RoleNames.Customer))
                {
                    form = "CustomerForm";
                    var viewModel = new CustomerUserViewModel()
                    {
                        Customer = customer,

                        ApplicationUser = _context.Users.SingleOrDefault(c => c.Id == customer.ApplicationUserId)
                    };
                    return View(form, viewModel);
                }
                else
                {
                    return View(form);
                }
            }
           
            var currentUserID = User.Identity.GetUserId();
            var userIsCustomer = _context.Customers.SingleOrDefault(c => c.ApplicationUserId == currentUserID);
            var userIsGarage = _context.Garages.SingleOrDefault(c => c.ApplicationUserId == currentUserID);
            var objectIsCustomer = _context.Customers.SingleOrDefault(c => c.ApplicationUserId == customer.ApplicationUserId);
            var objectIsGarage = _context.Garages.SingleOrDefault(c => c.ApplicationUserId == customer.ApplicationUserId);
            var userExists = _context.Users.SingleOrDefault(u => u.Id == customer.ApplicationUserId);
            //******** Come here if form is valid
            if (customer.ID == 0)
            {
                //user is a Customer already
                if (User.IsInRole(RoleNames.Customer))
                {
                    return View("AlreadyInRoleGeneral");
                }
                else
                {
                    //checking if the user has a role, a customer cannot be a garage in the same time
                    

                    if ((objectIsGarage == null || objectIsCustomer == null) && userExists!= null)
                    {
                        security.AddUserToRole(customer.ApplicationUserId, RoleNames.Customer);

                        _context.Customers.Add(customer);
                    }
                    else if (userIsCustomer == null || userIsGarage == null)
                       
                        //&&
                        //_context.Users.SingleOrDefault(u => u.Id == User.Identity.GetUserId()) != null
                    {
                        security.AddUserToRole(currentUserID, RoleNames.Customer);
                        customer.ApplicationUserId = currentUserID;

                        _context.Customers.Add(customer);
                    }

                    //if there is no userID on the file matching the one returned from the form

                    else if (_context.Users.SingleOrDefault(u => u.Id == customer.ApplicationUserId) == null)
                    {
                        return View("NeedToRegisterBefore");
                    }

                    //there is a user with this UserID in the customers or garage role
                    else
                    {
                        return View("AlreadyInRoleGeneral");
                    }
                }

            }
            //needs to be finished
            else
            {
                var customerInDB = _context.Customers.Single(c => c.ID == customer.ID);
                if (User.IsInRole(RoleNames.Customer))
                {
                    //Manually update the fields I want.
                    customerInDB.FirstName = customer.FirstName;
                    customerInDB.LastName = customer.LastName;
                    customerInDB.Address = customer.Address;
                    customerInDB.PhoneNumber = customer.PhoneNumber;
                    customerInDB.ApplicationUserId = User.Identity.GetUserId();
                    security.AddUserToRole(currentUserID, RoleNames.Customer);
                }
                else if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
                {
                    var UserId = _context.Users.SingleOrDefault(c => c.Id == customer.ApplicationUserId);
                    //Manually update the fields I want.
                    customerInDB.FirstName = customer.FirstName;
                    customerInDB.LastName = customer.LastName;
                    customerInDB.Address = customer.Address;
                    customerInDB.PhoneNumber = customer.PhoneNumber;
                    customerInDB.ApplicationUserId = customer.ApplicationUserId;
                    security.AddUserToRole(customer.ApplicationUserId, RoleNames.Customer);
                }

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

        [Authorize(Roles = RoleNames.AdministratorGarageOwnerCustomer)]
        public ActionResult Edit(int? Id)
        {
            var currentUserID = User.Identity.GetUserId();
            var customerInDB = _context.Customers.SingleOrDefault(c => c.ApplicationUserId == currentUserID);
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                customerInDB = _context.Customers.SingleOrDefault(c => c.ID == Id);
            }

            if (customerInDB == null)
                return HttpNotFound();

            return View("CustomerFormCustomer", customerInDB);
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