using System;
using System.ComponentModel.DataAnnotations;

namespace PW.InternalMoney.Models
{
    public class TransactionListItem
    {
        [Display(Name = "Id:")]
        public int TransactionId { get; set; }
        [Display(Name = "Date and time:")]
        public DateTime Date { get; set; }
        [Display(Name = "Correspondent name:")]
        public string CorrespondentName { get; set; }
        [Display(Name = "Transaction amount:")]
        public string TransactionAmount { get; set; }
    }
}