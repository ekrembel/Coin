using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinalProject.ViewModels
{
    public class UpdatePasswordViewModel
    {
        
        public string Username { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Password does not match!")]
        public string ConfirmNewPassword { get; set; }

        public UpdatePasswordViewModel()
        {
        }
    }
}
