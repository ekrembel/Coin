using System;
using System.Collections.Generic;
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
    public class SellController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ShareDbContext db;

        public SellController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ShareDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            db = dbContext;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            var user = db.ApplicationUser.Where(au => au.UserName == User.Identity.Name).ToList();
            List<Bought> shares = db.Bought.Where(s => s.AspNetUserId == user[0].Id).Where(s => s.IsOwned == true).ToList();
            List<string> symbols = new List<string>();

            foreach (Bought share in shares)
            {
                if (!symbols.Contains(share.Symbol))
                {
                    symbols.Add(share.Symbol);
                } 
            }

            SharesViewModel viewModel = new SharesViewModel(symbols);
            return View(viewModel);
        }

        
        public IActionResult ShareDetails(string symbol)
        {
            List<Bought> shares = db.Bought.Where(s => s.Symbol == symbol).Where(s => s.IsOwned == true).ToList();
            List<SharesViewModel> viewModels = new List<SharesViewModel>();
            foreach (Bought share in shares)
            {
                SharesViewModel model = new SharesViewModel
                {
                    companyName = share.companyName,
                    Symbol = share.Symbol,
                    latestPrice = share.latestPrice,
                    NumOfShare = share.NumOfShare,

                };
                viewModels.Add(model);
            }
            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Sell(string symbol)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri("https://cloud-sse.iexapis.com");
                    var response = await client.GetAsync($"/stable/stock/{symbol}/quote?token=");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawShare = JsonConvert.DeserializeObject<Bought>(stringResult);

                    ViewBag.name = rawShare.companyName;
                    ViewBag.symbol = symbol.ToUpper();
                    ViewBag.price = rawShare.latestPrice;

                    List<Bought> shares = db.Bought.Where(s => s.Symbol == symbol).Where(s => s.IsOwned == true).ToList();
                    List<SharesViewModel> viewModels = new List<SharesViewModel>();
                    foreach (Bought share in shares)
                    {
                        SharesViewModel model = new SharesViewModel
                        {
                            companyName = share.companyName,
                            Symbol = share.Symbol,
                            latestPrice = share.latestPrice,
                            NumOfShare = share.NumOfShare,

                        };
                        viewModels.Add(model);
                    }
                    return View(viewModels);
                }
                catch (HttpRequestException httpRequestException)
                {
                    ViewBag.errorMessage = $"We can't find a company with symbol '{symbol}'. Please enter the correct symbol.";
                    ViewBag.exceptionMessage = $"{httpRequestException.Message}";
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string symbol, int numOfShare)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri("https://cloud-sse.iexapis.com");
                    var response = await client.GetAsync($"/stable/stock/{symbol}/quote?token=pk_f7b30f305a8c4aef8eaec49711a8344e");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawShare = JsonConvert.DeserializeObject<SharesViewModel>(stringResult);

                    var user = db.ApplicationUser
                        .Where(au => au.UserName == User.Identity.Name)
                        .ToList();

                    

                    List<Bought> shareList = db.Bought.Where(s => s.Symbol == symbol).Where(s => s.NumOfShare == numOfShare).ToList();
                    shareList[0].IsOwned = false;
                    db.Bought.Update(shareList[0]);

                    DateTime date = DateTime.Now;

                    Sold share = new Sold
                    {
                        DateAndTime = date,
                        Symbol = shareList[0].Symbol,
                        companyName = shareList[0].companyName,
                        latestPrice = rawShare.latestPrice,
                        NumOfShare = shareList[0].NumOfShare,
                        Cost = shareList[0].latestPrice,
                        Profit = rawShare.latestPrice - shareList[0].latestPrice,
                        AspNetUserId = user[0].Id
                    };

                    user[0].Fund = user[0].Fund + numOfShare * (rawShare.latestPrice - shareList[0].latestPrice);

                    await userManager.UpdateAsync(user[0]);

                    db.Sold.Add(share);
                    db.SaveChanges();
                    ViewBag.message = "Sold!";

                    List<Bought> shares = db.Bought.Where(s => s.AspNetUserId == user[0].Id).Where(s => s.IsOwned == true).ToList();
                    List<string> symbols = new List<string>();
                    foreach (Bought item in shares)
                    {
                        if (!symbols.Contains(item.Symbol))
                        {
                            symbols.Add(item.Symbol);
                        }
                    }

                    SharesViewModel viewModel = new SharesViewModel(symbols);

                    return View(viewModel);
                }
                catch (HttpRequestException httpRequestException)
                {
                    var user = db.ApplicationUser
                        .Where(au => au.UserName == User.Identity.Name)
                        .ToList();
                    List<Bought> shares = db.Bought.Where(s => s.AspNetUserId == user[0].Id).Where(s => s.IsOwned == true).ToList();
                    List<string> symbols = new List<string>();
                    foreach (Bought share in shares)
                    {
                        if (!symbols.Contains(share.Symbol))
                        {
                            symbols.Add(share.Symbol);
                        }
                    }

                    SharesViewModel viewModel = new SharesViewModel(symbols);
                    ViewBag.errorMessage = "Failed. Please try again.";
                    ViewBag.exceptionMessage = $"Error: {httpRequestException.Message}";
                    return View(viewModel);
                }
            }

        }
    }
}
