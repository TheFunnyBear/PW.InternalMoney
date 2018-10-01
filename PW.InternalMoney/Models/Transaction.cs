using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW.InternalMoney.Models
{

    [Table("TRANSACTIONS")]
    public class Transaction
    {
        [Column("TRANSACTION_ID")]
        public int Id { get; set; }

        [Column("TRANSACTION_DATE")]
        public DateTime Date { get; set; }

        [Column("TRANSACTION_FROM_ID")]
        [ForeignKey("TransferFrom")]
        public int TransferFromId { get; set; }
        public BillingAccount TransferFrom { get; set; }

        [Column("TRANSACTION_TO_ID")]
        [ForeignKey("TransferTo")]
        public int TransferToId { get; set; }
        public BillingAccount TransferTo { get; set; }

        [Column("TRANSACTION_AMOUNT")]
        public Money TransactionAmount { get; set; }
    }
}