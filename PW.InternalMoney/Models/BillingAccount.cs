using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PW.InternalMoney.Models
{
    [Table("BILLING_ACCOUNTS")]
    public class BillingAccount
    {
        [Column("BILLING_ACCOUNT_ID")]
        public int Id { get; set; }

        [Column("BILLING_ACCOUNT_FIRSTNAME")]
        [Display(Name = "User first name is:")]
        public string FirstName { get; set; }

        [Column("BILLING_ACCOUNT_LASTNAME")]
        [Display(Name = "User last name is:")]
        public string LastName { get; set; }

        [Column("BILLING_ACCOUNT_BALANCE")]
        [Display(Name = "Current balance is:")]
        public Money Balance { get; set; }

        [NotMapped]
        public string FullUserName { get { return $"{FirstName} {LastName}"; } }

        public void InitAccount(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Balance = new Money(500, Currency.Pw);
        }
    }
}