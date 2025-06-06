﻿using System.ComponentModel.DataAnnotations;

namespace CodeTest.Transfers.DTO
{
    public class QuoteRequest
    {
        [Required]
        public string SellCurrency { get; set; }
        [Required]
        public string BuyCurrency { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
