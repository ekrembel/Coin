using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinalProject.ViewModels
{
    public class DeleteAccountViewModel
    {
        [Required]
        public string Password { get; set; }
        public string Reason { get; set; }

        public DeleteAccountViewModel()
        {
        }
    }
}
