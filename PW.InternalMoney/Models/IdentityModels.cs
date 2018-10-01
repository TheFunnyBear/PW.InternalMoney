using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PW.InternalMoney.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual BillingAccount BillingAccount { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<BillingAccount> BillingAccounts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.ComplexType<Money>()
                .Property(p => p.Serialized)
                .HasColumnName("TRANSACTION_AMOUNT");

            modelBuilder.ComplexType<Money>()
                .Property(p => p.Serialized)
                .HasColumnName("BILLING_ACCOUNT_BALANCE");

            modelBuilder.ComplexType<Money>().Ignore(p => p.Amount);
            modelBuilder.ComplexType<Money>().Ignore(p => p.SelectedCurrency);

            modelBuilder.Entity<IdentityUser>()
                .ToTable("USERS");

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("USERS");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}