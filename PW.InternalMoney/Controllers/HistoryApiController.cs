using PW.InternalMoney.DataProviders;
using PW.InternalMoney.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PW.InternalMoney.Controllers
{
    [Authorize]
    public class HistoryApiController : ApiController
    {
        // GET api/History
        public IEnumerable<TransactionHistoryItem> Get()
        {
            return GetTransactionItems();
        }

        // GET api/History/5
        public TransactionHistoryItem Get(int id)
        {
            return GetTransactionItems().ElementAt(id);
        }

        private IEnumerable<TransactionHistoryItem> GetTransactionItems()
        {
            var currentUser = BalanceProvider.GetCurentUserBillingAccount();

            using (var dataBase = new ApplicationDbContext())
            {
                var currentUserTransactionsList = dataBase.Transactions.Where(transaction =>
                        transaction.TransferFromId == currentUser.Id || transaction.TransferToId == currentUser.Id)
                    .OrderByDescending(transaction => transaction.Id)
                    .ToList();

                var billingAccounts = dataBase.BillingAccounts.ToList();
                var operationBalance = currentUser.Balance;
                var transactionHistory = currentUserTransactionsList.Select(transaction =>
                {
                    var historyItem = new TransactionHistoryItem();

                    historyItem.Date = transaction.Date;
                    if (transaction.TransferFromId == currentUser.Id)
                    {

                        var recipientAccount = billingAccounts.Single(account => account.Id == transaction.TransferToId);

                        historyItem.CorrespondentName = recipientAccount.FullUserName;
                        historyItem.Credit = transaction.TransactionAmount;
                        historyItem.Debit = new Money(0);
                        historyItem.Balance = operationBalance;
                        operationBalance += transaction.TransactionAmount;
                    }
                    else if (transaction.TransferToId == currentUser.Id)
                    {
                        var debitorAccount = billingAccounts.Single(account => account.Id == transaction.TransferFromId);

                        historyItem.CorrespondentName = debitorAccount.FullUserName;
                        historyItem.Debit = transaction.TransactionAmount;
                        historyItem.Credit = new Money(0);
                        historyItem.Balance = operationBalance;
                        operationBalance -= transaction.TransactionAmount;
                    }
                    return historyItem;
                }
                ).ToList();

                return transactionHistory;
            }
        }
    }
}