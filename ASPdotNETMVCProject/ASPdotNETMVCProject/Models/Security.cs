using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASPdotNETMVCProject.Models
{
    internal class Security
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        internal void AddUserToRole(string userName, string roleName)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            try
            {
                var user = UserManager.FindByName(userName);
                UserManager.AddToRole(user.Id, roleName);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        
    }
}