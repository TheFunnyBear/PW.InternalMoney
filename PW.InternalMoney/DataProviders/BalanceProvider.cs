using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PW.InternalMoney.Models;
using System.Web;

namespace PW.InternalMoney.DataProviders
{
    public static class BalanceProvider
    {
        public static BillingAccount GetCurentUserBillingAccount()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId());
            return currentUser.BillingAccount;
            
        }
    }
}