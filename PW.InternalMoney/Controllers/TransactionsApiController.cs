using PW.InternalMoney.DataProviders;
using PW.InternalMoney.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PW.InternalMoney.Controllers
{
    [Authorize]
    public class TransactionsApiController : ApiController
    {
        // GET api/TransactionsApi
        public IEnumerable<TransactionListItem> Get()
        {
            return GetTransactionItems();
        }

        // GET api/TransactionsApi/5
        public TransactionListItem Get(int id)
        {
            return GetTransactionItems().ElementAt(id);
        }

        private IEnumerable<TransactionListItem> GetTransactionItems()
        {
            var currentUser = BalanceProvider.GetCurentUserBillingAccount();

            using (var dataBase = new ApplicationDbContext())
            {
                var currentUserTransactionsList = dataBase.Transactions.Where(transaction =>
                        transaction.TransferFromId == currentUser.Id)
                    .OrderByDescending(transaction => transaction.Id)
                    .ToList();

                var billingAccounts = dataBase.BillingAccounts.ToList();

                var transactionListItems = currentUserTransactionsList.Select(transaction =>
                {
                    var item = new TransactionListItem
                    {
                        Date = transaction.Date,
                        TransactionId = transaction.Id,
                        TransactionAmount = transaction.TransactionAmount.ToString()
                    };
                    var recipientAccount = billingAccounts.Single(account => account.Id == transaction.TransferToId);
                    item.CorrespondentName = recipientAccount.FullUserName;
                    return item;
                });

                return transactionListItems;
            }
        }
    }
}
