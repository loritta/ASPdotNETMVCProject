using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNETMVCProject.Models
{
    public class RoleNames
    {
        public const string GarageOwner = "CanManageGarageInfo";
        public const string Customer = "CanManageCustomerInfo";
        public const string Administrator = "CanManageTheWebSite";
        public const string AdministratorGarageOwner = "CanManageTheWebSite, CanManageGarageInfo";
        public const string AdministratorGarageOwnerCustomer = "CanManageTheWebSite, CanManageGarageInfo, CanManageCustomerInfo";

    }
}