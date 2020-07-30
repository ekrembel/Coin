using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinalProject.Data;
using MyFinalProject.Models;
using MyFinalProject.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFinalProject.Controllers
{
    public class UsersController : Controller
    {
        // GET: /<controller>/
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ShareDbContext db;

        public UsersController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ShareDbContext dbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            db = dbContext;
        }

 

        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Summary", "Transactions");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");

            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        public IActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<ApplicationUser> users = db.ApplicationUser.Where(au => au.UserName == model.Username).ToList();
                ApplicationUser theUser = users[0];
                var result = await userManager.CheckPasswordAsync(theUser, model.OldPassword);
                
                if (result)
                {
                    var resultUpdate = await userManager.ChangePasswordAsync(theUser, model.OldPassword, model.NewPassword);
                    if (resultUpdate.Succeeded)
                    {
                        ViewBag.status = "success";
                        ViewBag.message = "Congrats! Your password has been updated.";
                        return View();
                    }
                    foreach (var error in resultUpdate.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View(model);
                    }
                }
            }
            return View(model);
        }

        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<ApplicationUser> users = db.ApplicationUser.Where(au => au.UserName == User.Identity.Name).ToList();
                if (users.Count() > 0)
                {
                    Feedback feedback = new Feedback
                    {
                        Reason = model.Reason
                    };
                    db.Feedbacks.Add(feedback);

                    ApplicationUser user = users[0];

                    List<Bought> boughts = db.Bought.Where(b => b.AspNetUserId == user.Id).ToList();
                    foreach (Bought share in boughts)
                    {
                        db.Bought.Remove(share);
                    }

                    List<Sold> solds = db.Sold.Where(b => b.AspNetUserId == user.Id).ToList();
                    foreach (Sold share in solds)
                    {
                        db.Sold.Remove(share);
                    }
                    db.ApplicationUser.Remove(user);
                    db.SaveChanges();
                    await signInManager.SignOutAsync();
                    string message = "We're sorry to see you go!";
                    return RedirectToAction("Index", "Home", new { message });
                }
                else
                {
                    ViewBag.error = "Failed. Please try again.";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword(string[] message)
        {
            if (message.Count() > 0)
            {
                ViewBag.message = message[0];
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string emailAddress)
        {
            if (emailAddress != null)
            {
                var user = await userManager.FindByEmailAsync(emailAddress);

                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    ViewBag.message = "Found the user.";

                    string email = emailAddress;
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("SetPassword", "Sell", new { email = email, token = token }, Request.Scheme);

                    string[] data = { email, token };
                    return RedirectToAction("SetPassword", "Users", new { data });

                }
                else
                {

                    ViewBag.message = "Your email has not been confirmed yet.";
                    return View();
                }
            }
            ViewBag.message = "Please provide your email.";
            return View();

        }

        
        public IActionResult SetPassword(string[] data)
        {
            ViewBag.email = data[0];
            ViewBag.token = data[1];
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SetPassword(ResetPasswordViewModel model)
        {
            if (model.Token == null)
            {
                string[] message = { "Please start again" };
                return RedirectToAction("ForgotPassword", "Users", new { message });
            }
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.status = "success";
                    ViewBag.message = "Congrats! Your password has been reset.";
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(model);
                }
               
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(UsersViewModel usersViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = usersViewModel.Username,
                    Email = usersViewModel.Email,
                    Fund = 10000,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, usersViewModel.Password);

                if (result.Succeeded)
                {
                    string message = "Welcome To Coin!";
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Summary", "Transactions", new { message });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(usersViewModel);

        }

        public IActionResult FundAmount(string messageFail, string messageSuccess)
        {
            var user = db.ApplicationUser
                .Where(au => au.UserName == User.Identity.Name)
                .ToList();

            ViewBag.fund = user[0].Fund;

            if (messageSuccess != null)
            {
                ViewBag.messageSuccess = messageSuccess;
            }
            else
            {
                ViewBag.messageFail = messageFail;
            }
            return View();
        }

        [HttpPost]
        public IActionResult FundAmount(double amount)
        {
            double amountToAdd = amount;
            return RedirectToAction("AddFund", "Users", new { amountToAdd });
        }

        public IActionResult AddFund(double amountToAdd)
        {
            ViewBag.fee = amountToAdd / 1000.00;
            ViewBag.amount = amountToAdd;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFund(AddFundViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                List<ApplicationUser> users = db.ApplicationUser.Where(au => au.UserName == User.Identity.Name).ToList();
                if (users.Count() > 0)
                {
                    ApplicationUser user = users[0];
                    user.Fund = user.Fund + model.Amount;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        string messageSuccess = "The amount has been added to your fund.";
                        return RedirectToAction("FundAmount", "Users", new { messageSuccess });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            string messageFail = "Please start again.";
            return RedirectToAction("FundAmount", "Users", new { messageFail });
        }
    }
}
