using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPdotNETMVCProject.Models;
using ASPdotNETMVCProject.ViewModels;
using Microsoft.AspNet.Identity;

namespace ASPdotNETMVCProject.Controllers
{
    public class GaragesController : Controller
    {
        //DB Context Object
        private ApplicationDbContext _context;
        private Security security;
        //class Constructor
        public GaragesController()
        {
            _context = new ApplicationDbContext();
            security = new Security();
        }

        [AllowAnonymous]
        // GET: Garages
        public ActionResult Index(string SearchString, string sort)
        {
            var garages = _context.Garages.ToList();
            //Check for user
            string view = "ReadOnlyIndex";

            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Index";
            }

            //saving the search to view
            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewBag.SortbyAddress = sort == "address" ? "address_desc" : "address";

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
                    garages = garages.Where(g => g.Name.ToLower().Contains(SearchString) ||
                    g.Address.ToLower().Contains(SearchString)).ToList();

                }
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
            return View(view, garages);

        }
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            string view = "ReadOnlyDetails";
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                view = "Details";
            }
            var garage = _context.Garages.
                SingleOrDefault(c => c.ID == id);
            if (garage == null)
                return HttpNotFound();
            else
                return View(view, garage);
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
        [Authorize]
        public ActionResult New()
        {
            var form = "GarageFormUser";

            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                form = "CustomerForm";
                var viewModel = new ServicesInGarageViewModel()
                {
                    Garage = new Garage(),
                    Services = (from data in _context.Garages
                                join data2 in _context.GarageServices on data.ID equals data2.GarageID
                                join data3 in _context.Services on data2.ServiceID equals data3.ID
                                select data3).ToList()

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
        [Authorize(Roles = RoleNames.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Garage garage)
        {

            if (!ModelState.IsValid)
            {
                var form = "GarageFormUser";
                if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner) || User.IsInRole(RoleNames.Customer))
                {
                    form = "GarageForm";
                    var viewModel = new ServicesInGarageViewModel()
                    {
                        Garage = garage,
                        Services = (from data in _context.Garages
                                    join data2 in _context.GarageServices on data.ID equals data2.GarageID
                                    join data3 in _context.Services on data2.ServiceID equals data3.ID
                                    select data3).ToList()

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
            var objectIsCustomer = _context.Customers.SingleOrDefault(c => c.ApplicationUserId == garage.ApplicationUserId);
            var objectIsGarage = _context.Garages.SingleOrDefault(c => c.ApplicationUserId == garage.ApplicationUserId);
            var userExists = _context.Users.SingleOrDefault(u => u.Id == garage.ApplicationUserId);
            //******** Come here if form is valid
            if (garage.ID == 0)
            {
                //_context.Garages.Add(garage);
                if (User.IsInRole(RoleNames.GarageOwner))
                {
                    return View("AlreadyInRoleGeneral");
                }
                else
                {
                    //checking if the user has a role, a customer cannot be a garage in the same time


                    if ((objectIsGarage == null || objectIsCustomer == null) && userExists != null)
                    {
                        security.AddUserToRole(garage.ApplicationUserId, RoleNames.GarageOwner);

                        _context.Garages.Add(garage);
                    }
                    else if (userIsCustomer == null || userIsGarage == null)

                    //&&
                    //_context.Users.SingleOrDefault(u => u.Id == User.Identity.GetUserId()) != null
                    {
                        security.AddUserToRole(currentUserID, RoleNames.GarageOwner);
                        garage.ApplicationUserId = currentUserID;

                        _context.Garages.Add(garage);
                    }

                    //if there is no userID on the file matching the one returned from the form

                    else if (_context.Users.SingleOrDefault(u => u.Id == garage.ApplicationUserId) == null)
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
            else
            {
                var garageInDB = _context.Garages.Single(g => g.ID == garage.ID);
                
                    var UserId = _context.Users.SingleOrDefault(c => c.Id == garage.ApplicationUserId);
                    //Manually update the fields I want.
                    garageInDB.Name = garage.Name;
                    garageInDB.Address = garage.Address;
                    garageInDB.PhoneNumber = garage.PhoneNumber;
                    garageInDB.ApplicationUserId = garage.ApplicationUserId;
                    security.AddUserToRole(garage.ApplicationUserId, RoleNames.GarageOwner);
            
            }
            _context.SaveChanges();
            if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
            {
                return RedirectToAction("Index", "Garages");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = RoleNames.AdministratorGarageOwner)]
        public ActionResult Edit(int? Id)
        {
            var currentUserID = User.Identity.GetUserId();
            var garageInDB = _context.Garages.SingleOrDefault(c => c.ApplicationUserId == currentUserID);
           
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
}