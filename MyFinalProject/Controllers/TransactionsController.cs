using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Data;
using MyFinalProject.Models;
using MyFinalProject.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFinalProject.Controllers
{
    
    public class TransactionsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ShareDbContext db;

        public TransactionsController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ShareDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            db = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Summary(string message)
        {
            double total = 0;
            var user = db.ApplicationUser.Where(au => au.UserName == User.Identity.Name).ToList();
            List<Bought> shares = db.Bought.Where(s => s.AspNetUserId == user[0].Id).Where(s => s.IsOwned == true).ToList();

            foreach (Bought share in shares)
            {
                total += (share.latestPrice * share.NumOfShare);
            }

            double cash = user[0].Fund - total;
            ViewBag.fund = user[0].Fund;
            ViewBag.total = total;
            ViewBag.cash = cash;

            if (message != null)
            {
                ViewBag.message = message;
            }

            return View(shares);
        }

        public IActionResult History()
        {
            double totalProfit = 0;
            var user = db.ApplicationUser.Where(au => au.UserName == User.Identity.Name).ToList();
            List<Sold> sharesSold = db.Sold.Where(s => s.AspNetUserId == user[0].Id).ToList();
            foreach (Sold share in sharesSold)
            {
                totalProfit += share.Profit;
            }

            ViewBag.total = totalProfit;

            return View(sharesSold);
        }
    }
}
