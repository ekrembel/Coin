using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinalProject.ViewModels
{
    public class BuyViewModel
    {
        [Required(ErrorMessage = "Please enter the symbol of the company.")]
        public string Symbol { get; set; }

        [Required(ErrorMessage = "Please enter the number share you want to buy.")]
        public int NumOfShare { get; set; }

        public BuyViewModel()
        {
        }
    }
}
