using PW.InternalMoney.DataProviders;
using PW.InternalMoney.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace PW.InternalMoney.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int? id)
        {
            var billingAccount = BalanceProvider.GetCurentUserBillingAccount();
            using (var dataBase = new ApplicationDbContext())
            {
                var dbBillingAccounts = dataBase.BillingAccounts.ToList();
                var billingAccountsList = dbBillingAccounts.OrderBy(account => account.FirstName)
                    .Select(account => new BillingUser { id =  account.Id, name = account.FullUserName})
                    .ToList();
                ViewBag.BillingAccountsList = billingAccountsList;
                decimal defaultValue = 0.01m;
                ViewBag.TransactionAmount = new Money(defaultValue).Amount;

                if (billingAccount.Balance <= new Money(0))
                {
                    return RedirectToAction("NotEnoughFunds");
                }

                ViewBag.MaxTransaction = billingAccount.Balance.Amount;

                if (id != null)
                {
                    var repeatTransaction = dataBase.Transactions.SingleOrDefault(transaction => transaction.Id == id);
                    if (repeatTransaction != null)
                    {
                        var selectAcountInList = billingAccountsList.SingleOrDefault(account => account.id == repeatTransaction.TransferToId);
                        ViewBag.SelectedRecipientId = selectAcountInList.id;
                        ViewBag.InitialRecipient = selectAcountInList.name;
                        ViewBag.TransactionAmount = (billingAccount.Balance < repeatTransaction.TransactionAmount) ? billingAccount.Balance.Amount : repeatTransaction.TransactionAmount.Amount;
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public JsonResult CommitTransaction(string amount, int selectedRecipient)
        {
            if (string.IsNullOrEmpty(amount))
            {
                return Json(new
                {
                    success = false,
                    responseText = $"Not valid amount [{amount}]. Please check form fields."
                }, JsonRequestBehavior.AllowGet);
            }
            var transaction = new Transaction();
            amount = amount.Replace(',', '.');
            var amountValue = new Money(Decimal.Parse(amount, CultureInfo.InvariantCulture));

            using (var dataBase = new ApplicationDbContext())
            {
                var billingAccounts = dataBase.BillingAccounts.ToList();
                var sendToRecipient = billingAccounts.SingleOrDefault(account => account.Id.Equals(selectedRecipient));
                if (sendToRecipient != null)
                {
                    var billingAccount = BalanceProvider.GetCurentUserBillingAccount();
                    if (billingAccount.Balance < amountValue)
                    {
                        return Json(new
                        {
                            success = false,
                            responseText = $"Not valid amount: [{amountValue}]. Balance is: [{billingAccount.Balance}]. Please check form fields."
                        });
                    }

                    var id = BalanceProvider.GetCurentUserBillingAccount().Id;
                    if (id == selectedRecipient)
                    {
                        return Json(new
                        {
                            success = false,
                            responseText = $"You can not transfer money to yourself!. Please check form fields."
                        });
                    }

                    var currntAccount = dataBase.BillingAccounts.Single(account => account.Id == id);
                    var recipientAccount = dataBase.BillingAccounts.Single(account => account.Id == sendToRecipient.Id);

                    transaction.TransferFromId = currntAccount.Id;
                    transaction.TransferFrom = currntAccount;
                    transaction.TransferToId = sendToRecipient.Id;
                    transaction.TransferTo = sendToRecipient;
                    transaction.TransactionAmount = amountValue;
                    transaction.Date = DateTime.Now;
                    dataBase.Transactions.Add(transaction);

                    currntAccount.Balance -= amountValue;
                    recipientAccount.Balance += amountValue;
                    dataBase.SaveChanges();


                    return Json(new { success = true, responseText = transaction.Id });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        responseText = "Can't find user. Please check form fields."
                    });
                }
            }
        }

        public ActionResult TransactionComplete(int? id)
        {
            if (id != null)
            {
                var billingAccount = BalanceProvider.GetCurentUserBillingAccount();
                using (var dataBase = new ApplicationDbContext())
                {
                    var curentUserTransactions = dataBase.Transactions.Where(transaction => transaction.TransferFromId == billingAccount.Id).ToList();
                    var currentTransaction = curentUserTransactions.SingleOrDefault(transaction => transaction.Id == id);
                    if(currentTransaction != null)
                    {
                        var item = new TransactionListItem
                        {
                            TransactionId = currentTransaction.Id,
                            Date = currentTransaction.Date,
                            TransactionAmount = currentTransaction.TransactionAmount.ToString()
                        };

                        var billingAccounts = dataBase.BillingAccounts.ToList();
                        var correspondent = billingAccounts.SingleOrDefault(account => account.Id.Equals(currentTransaction.TransferToId));
                        if (correspondent != null)
                        {
                            item.CorrespondentName = correspondent.FullUserName;
                            return View(item);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult History()
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

                return View(transactionHistory);
            }
        }

        public ActionResult List()
        {
            var currentUser = BalanceProvider.GetCurentUserBillingAccount();

            using (var dataBase = new ApplicationDbContext())
            {
                var currentUserTransactionsList = dataBase.Transactions.Where(transaction =>
                        transaction.TransferFromId == currentUser.Id )
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
                
                return View(transactionListItems);
            }
        }

        public ActionResult NotEnoughFunds()
        {
            return View();
        }

        public ActionResult CanNotTransferMoneyToYourself()
        {
            return View();
        }

    }
}