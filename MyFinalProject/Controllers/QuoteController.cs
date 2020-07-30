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
    public class QuoteController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ShareDbContext db;

        public QuoteController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ShareDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            db = dbContext;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult All()
        {
            List<Company> allCompanies = db.Companies.ToList();

            List<SharesViewModel> companies = new List<SharesViewModel>();

            foreach (Company company in allCompanies)
            {
                SharesViewModel viewModel = new SharesViewModel
                {
                    Symbol = company.Symbol,
                    companyName = company.Name
                };

                companies.Add(viewModel);
            }
            return View(companies);
        }

        //[HttpPost]
        //public IActionResult LoadAll()
        //{
        //    List<SharesViewModel> companies = CompanyData.Companies();
        //    foreach (SharesViewModel viewModel in companies)
        //    {
        //        Company company = new Company
        //        {
        //            Symbol = viewModel.Symbol,
        //            Name = viewModel.companyName
        //        };

        //        db.Companies.Add(company);
        //    }
        //    db.SaveChanges();
        //    return View();
        //}

        

        [HttpPost]
        //[Route("/Quote/Quoted")]
        public async Task<IActionResult> Quoted(string symbol)
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

                    SharesViewModel share = new SharesViewModel
                    {
                        Symbol = symbol.ToUpper(),
                        companyName = rawShare.companyName,
                        latestPrice = rawShare.latestPrice
                    };
                    
                    return View(share);
                }
                catch (HttpRequestException httpRequestException)
                {
                    ViewBag.errorMessage = $"We can't find a company with symbol '{symbol}'. Please enter the correct symbol.";
                    ViewBag.exceptionMessage = $"{httpRequestException.Message}";
                    return View();
                }
            }
        }


    }
}
