using ASPdotNETMVCProject.Models;
using ASPdotNETMVCProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNETMVCProject.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext _context;

        //class Constructor
        public MessagesController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {


            var viewModel = new MessageCustomerGaragesViewModel()
            {
                Message = new Message(),
                Customer = (Customer)Session["customerLoggedIn"],
                Garages = _context.Garages.ToList()

            };


            //if it's an admin or a garage or a user who has the right to add customers
            //if (User.IsInRole(RoleNames.Customer))


            return View("MessageForm", viewModel);

            /* if (User.IsInRole(RoleNames.Administrator) || User.IsInRole(RoleNames.GarageOwner))
             {
                 return View("CustomerForm", customer);
             }
             else
             {
                 return View("CustomerFormUser", customer);
             }*/
        }
        //[Authorize(Roles = RoleNames.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Message message)
        {

            if (!ModelState.IsValid)
            {
                //The form is not valid --> return same form to the user
                var viewModel = new MessageCustomerGaragesViewModel
                {
                    Message = new Message(),
                    Customer = new Customer(),
                    Garages = _context.Garages.ToList()
                };

                return View("MessageForm", viewModel);
            }
            //******** Come here if form is valid
            if (message.ID == 0)
            {
                _context.Messages.Add(message);
            }
            else
            {
                var messageInDB = _context.Messages.Single(c => c.ID == message.ID);


                messageInDB.CustomerID = message.CustomerID;
                messageInDB.GarageID = message.GarageID;
                messageInDB.Title = message.Title;
                messageInDB.Contents = message.Contents;
                

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}