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
    public class SearchController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ShareDbContext db;

        public SearchController(UserManager<ApplicationUser> userManager,
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
            return View();
        }

        [HttpPost]
        public IActionResult Index(string name)
        {
            List<Company> allCompanies = db.Companies.ToList();
            List<SharesViewModel> companies = new List<SharesViewModel>();
            foreach (var company in allCompanies)
            {
                if (company.Name.ToUpper().Contains(name.ToUpper()))
                {
                    SharesViewModel viewModel = new SharesViewModel
                    {
                        Symbol = company.Symbol,
                        companyName = company.Name
                    };
                    companies.Add(viewModel);
                }
            }
            return View(companies);
        }
    }
}
