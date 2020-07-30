using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinalProject.ViewModels
{
    public class ResetPasswordViewModel
    {
        //[Required(ErrorMessage = "Please provide your email.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Password does not match!")]
        public string ConfirmNewPassword { get; set; }

        public string Token { get; set; }

        public ResetPasswordViewModel()
        {
        }

    }
}
