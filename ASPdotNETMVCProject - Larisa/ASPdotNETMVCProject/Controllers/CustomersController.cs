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
            var customers = _context.Customers.ToList();
            return View(customers);
        }
        public ActionResult Details(int id)
        {

            var customer = _context.Customers.               
                SingleOrDefault(c => c.ID == id);
            if (customer == null)
                return HttpNotFound();
            else
                return View(customer);
        }
        public ActionResult New()
        {

            var customer = new Customer();
           
            return View("CustomerForm", customer);
        }
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