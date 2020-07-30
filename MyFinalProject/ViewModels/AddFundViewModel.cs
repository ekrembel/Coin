using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinalProject.ViewModels
{
    public class AddFundViewModel
    {
        [Required]
        public double Amount { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Your card number must be 16 digits.")]
        public string CardNumber { get; set; }

        [Required]
        public string CCV { get; set; }

        [Required]
        public int ExpirationYear { get; set; }

        public AddFundViewModel()
        {
        }
    }
}
