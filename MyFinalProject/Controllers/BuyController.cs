using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Data;
using MyFinalProject.Models;
using MyFinalProject.ViewModels;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFinalProject.Controllers
{
    public class BuyController : Controller
    {
        private readonly ShareDbContext db;
        private UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        

        public BuyController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ShareDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            db = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(string symbol)
        {
            BuyViewModel viewModel = new BuyViewModel();
            ViewBag.symbol = symbol;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BuyViewModel model)
        {
            if (ModelState.IsValid & model.NumOfShare > 0)
            {
                using (var client = new HttpClient())
                {
                    try
                    {

                        client.BaseAddress = new Uri("https://cloud-sse.iexapis.com");
                        var response = await client.GetAsync($"/stable/stock/{model.Symbol}/quote?token=");
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();
                        var rawShare = JsonConvert.DeserializeObject<SharesViewModel>(stringResult);

                        var user = db.ApplicationUser
                            .Where(au => au.UserName == User.Identity.Name)
                            .ToList();

                        double total = 0;
                        List<Bought> shares = db.Bought.Where(s => s.AspNetUserId == user[0].Id).Where(s => s.IsOwned == true).ToList();
                        foreach (Bought item in shares)
                        {
                            total += (item.latestPrice * item.NumOfShare);
                        }

                        double cash = user[0].Fund - total;

                        double cost = rawShare.latestPrice * model.NumOfShare;

                        if (cash >= cost)
                        {
                            DateTime date = DateTime.Now;

                            Bought share = new Bought
                            {
                                TransactionType = "Buy",
                                DateAndTime = date,
                                Symbol = model.Symbol.ToUpper(),
                                companyName = rawShare.companyName,
                                latestPrice = rawShare.latestPrice,
                                NumOfShare = model.NumOfShare,
                                IsOwned = true,
                                AspNetUserId = user[0].Id
                            };


                            db.Bought.Add(share);
                            db.SaveChanges();
                            ViewBag.message = "Bought!";
                            return View();
                        }
                      
                        ViewBag.cashErrorMessage = "Failed. Total cost exceeds your available cash.";
                        return View();


                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        ViewBag.errorMessage = "Failed. Please try again.";
                        ViewBag.exceptionMessage = $"Error: {httpRequestException.Message}";
                        return View();
                    }
                }

            }
            return View(model);
        }
    }
}
