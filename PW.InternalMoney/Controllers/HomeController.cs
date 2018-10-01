using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PW.InternalMoney.DataProviders;
using PW.InternalMoney.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PW.InternalMoney.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BillingInfo()
        {
            var billingAccount = BalanceProvider.GetCurentUserBillingAccount();
            return View(billingAccount);
            /*
            using (var dataBase = new ApplicationDbContext())
            {
                var billingAccoun = dataBase.BillingAccounts.SingleOrDefault(account => account.Id == billingAccountId);
                if (billingAccoun != null)
                {
                    return View();
                }
            }
            return View(new BillingAccount());
            */
            /*
        using (var dataBase = new ApplicationDbContext())
        {
            dataBase.
            var galleryItem = dataBase.Images.SingleOrDefault(item => item.Id == id);
            var fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}.jpg";
            return File(galleryItem.Image, "image/jpg", fileName);
        }

        return View();
        */
        }
    }
}