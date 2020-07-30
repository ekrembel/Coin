using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFinalProject.Models;

namespace MyFinalProject.ViewModels
{
    public class SharesViewModel
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Symbol { get; set; }
        public string companyName { get; set; }
        public int NumOfShare { get; set; }
        public double latestPrice { get; set; }
        public bool IsOwned { get; set; }

        public List<SelectListItem> Symbols { get; set; }

        public SharesViewModel(List<string> symbols)
        {
            Symbols = new List<SelectListItem>();

            foreach (string symbol in symbols)
            {
                Symbols.Add(new SelectListItem
                {
                    Value = symbol,
                    Text = symbol
                });
            }
        }

        public SharesViewModel(string name, string symbol)
        {
            companyName = name;
            Symbol = symbol;
        }

        public SharesViewModel()
        {
        }
    }
}
