using System;
using System.ComponentModel.DataAnnotations;

namespace PW.InternalMoney.Models
{
    public class TransactionHistoryItem
    {
        [Display(Name = "Date and time:")]
        public DateTime Date { get; set; }
        [Display(Name = "Correspondent name:")]
        public string CorrespondentName { get; set; }
        [Display(Name = "Debit:")]
        public Money Debit { get; set; }
        [Display(Name = "Credit:")]
        public Money Credit { get; set; }
        [Display(Name = "Balance:")]
        public Money Balance { get; set; }
    }
}