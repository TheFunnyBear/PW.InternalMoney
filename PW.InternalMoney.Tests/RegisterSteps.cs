using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;


namespace PW.InternalMoney.Tests
{

    [Binding]
    public sealed class RegisterSteps
    {
        private const string DefaultPassword = "4D32DAF5251C^z";
        private readonly IBrowserNavigator _browser;
        private readonly ITestsConstants _testsConstants;
        private readonly ILinkCliker _linkCliker;
        private readonly IButtonCliker _buttonCliker;
        private readonly List<Person> _users = new List<Person>()
            {
                new Person("Althea", "Goodwin"),
                new Person("Micah", "Alberti"),
                new Person("Coit", "Albertson"),
                new Person("Frank", "Albertson"),
                new Person("Jack", "Albertson"),
                new Person("J.Grant", "Albrecht"),
                new Person("Budd", "Albright"),
                new Person("Tucker", "Albrizzi"),
                new Person("Ghazi", "Albuliwi"),
                new Person("Chris", "Alcaide"),
                new Person("Alejandro", "Alcondez"),
                new Person("Todd", "Alcott"),
                new Person("Alan", "Alda"),
                new Person("Antony", "Alda"),
                new Person("Robert", "Alda"),
                new Person("Norman", "Alden"),
                new Person("Tom", "Aldredge"),
                new Person("Fred", "Aldrich"),
                new Person("Kevin", "Alejandro"),
                new Person("Grant", "Aleksander"),
                new Person("Aki", "Aleong"),
                new Person("John", "Ales"),
                new Person("Frank", "Alesia"),
                new Person("Frank", "Aletter"),
                new Person("Ben", "Alexander"),
                new Person("Christian", "Alexander"),
                new Person("Cris", "Alexander"),
                new Person("Flex", "Alexander"),
                new Person("Jace", "Alexander"),
                new Person("Jason", "Alexander"),
                new Person("John", "Alexander"),
                new Person("Max", "Alexander"),
                new Person("Pico", "Alexander")
            };

        private decimal PreviousBalance {get; set; }

        public RegisterSteps(
            ITestsConstants testsConstants,
            IBrowserNavigator browser,
            ILinkCliker linkCliker,
            IButtonCliker buttonCliker)
        {
            _testsConstants = testsConstants;
            _browser = browser;
            _linkCliker = linkCliker;
            _buttonCliker = buttonCliker;
        }

        [Given(@"Navigate to main page")]
        public void GivenNavigateToMainPage()
        {
            _browser.Open();
            _browser.Navigate(_testsConstants.WebSiteHttpAddress);
        }

        [Given(@"Navigate to register form")]
        public void GivenNavigateToRegisterForm()
        {
            const string linkText = "Register";
            _linkCliker.ClickOnLinkWithText(linkText);
        }

        [Given(@"Entered User first name")]
        public void GivenEnteredUserFirstName()
        {
            SetTextInTextBoxWithId("FirstName", "Denis");
        }

        [Given(@"Entered User last name")]
        public void GivenEnteredUserLastName()
        {
            SetTextInTextBoxWithId("LastName", "Kosolapov");
        }

        [Given(@"Entered User email")]
        public void GivenEnteredUserEmail()
        {
            SetTextInTextBoxWithId("Email", "denis@kosolapov.info");
        }

        [Given(@"Entered Password")]
        public void GivenEnteredPassword()
        {
            SetTextInTextBoxWithId("Password", DefaultPassword);
        }

        [Given(@"Entered Confirm password")]
        public void GivenEnteredConfirmPassword()
        {
            SetTextInTextBoxWithId("ConfirmPassword", DefaultPassword);
        }

        [When(@"Press Register")]
        public void WhenPressRegister()
        {
            const string butonText = "Register";
            _buttonCliker.ClickOnButtonWithValue(butonText);
        }

        [When(@"Press Log off")]
        public void WhenPressLogOff()
        {
            const string linkText = "Log off";
            _linkCliker.ClickOnLinkWithText(linkText);
        }


        [Then(@"User account created")]
        public void ThenUserAccountCreated()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Navigate to login form")]
        public void GivenNavigateToLoginForm()
        {
            const string linkText = "Log in";
            _linkCliker.ClickOnLinkWithText(linkText);
        }

        [When(@"Press Login")]
        public void WhenPressLogin()
        {
            const string butonText = "Log in";
            _buttonCliker.ClickOnButtonWithValue(butonText);
        }

        [Then(@"User Logged in")]
        public void ThenUserLoggedIn()
        {
            const string helloMessage = "Hello Denis Kosolapov!";

            IList<IWebElement> links = _browser.GetDriver().FindElements(By.TagName("a"));
            var linkWithUserName =  links.SingleOrDefault(element => element.Text.Equals(helloMessage, StringComparison.OrdinalIgnoreCase));

            Assert.IsNotNull(linkWithUserName, $"Can't find [{helloMessage}] link in page.");
        }

        [When(@"Create some users")]
        public void WhenCreateSomeUsers()
        {
            GivenNavigateToMainPage();
            foreach (var user in _users)
            {
                GivenNavigateToRegisterForm();
                TimeWaiting.WaitSeconds(1);
                SetTextInTextBoxWithId("FirstName", user.FirstName);
                SetTextInTextBoxWithId("LastName", user.LastName);
                SetTextInTextBoxWithId("Email", $"{user.FirstName}.{user.LastName}@gmail.com");
                SetTextInTextBoxWithId("Password", DefaultPassword);
                SetTextInTextBoxWithId("ConfirmPassword", DefaultPassword);
                WhenPressRegister();
                WhenPressLogOff();
            }
        }

        [Then(@"Users created")]
        public void ThenUsersCreated()
        {

        }

        [Given(@"Login User(.*)")]
        public void GivenLoginUser(int p0)
        {
            var user = _users.ElementAt(p0);
            GivenNavigateToLoginForm();
            TimeWaiting.WaitSeconds(1);
            SetTextInTextBoxWithId("Email", $"{user.FirstName}.{user.LastName}@gmail.com");
            SetTextInTextBoxWithId("Password", DefaultPassword);
            WhenPressLogin();
        }

        [When(@"Login User(.*)")]
        public void WhenLoginUser(int p0)
        {
            GivenLoginUser(p0);
            TimeWaiting.WaitSeconds(2);
        }

        [Given(@"Remember Balance")]
        public void GivenRememberBalance()
        {
            PreviousBalance = GetBalance();
        }

        [Given(@"Logout User(.*)")]
        public void GivenLogoutUser(int p0)
        {
            WhenPressLogOff();
        }

        [Given(@"Navigate to create")]
        public void GivenNavigateToCreate()
        {
            const string linkText = "Create";
            _linkCliker.ClickOnLinkWithText(linkText);
        }

        [When(@"Transfer to User (.*) money (.*)")]
        public void WhenTransferToUserMoney(int p0, Decimal p1)
        {
            GivenTransferToUserMoney(p0, p1);
        }

        [Given(@"Transfer to User (.*) money (.*)")]
        public void GivenTransferToUserMoney(int p0, Decimal p1)
        {
            var TransactionAmountField = _browser.GetDriver().FindElement(By.Name("transactionamount"));
            Assert.IsNotNull(TransactionAmountField, $"Can't find amount field in page.");

            var recipientAutocompleteField = _browser.GetDriver().FindElement(By.Id("recipientAutocompleteField"));
            Assert.IsNotNull(recipientAutocompleteField, $"Can't find auto complete field in page.");

            recipientAutocompleteField.Click();
            for (int i = 0; i < 9; i++)
            {
                new Actions(_browser.GetDriver()).SendKeys(Keys.Tab).Perform();
                TimeWaiting.WaitSeconds(1);
            }
            var charsInUserName = _users.ElementAt(p0).FullName.ToCharArray();
            
            for (int i = 0; i < charsInUserName.Count() - 3; i++)
            {
                var charInUserName = charsInUserName[i];
                new Actions(_browser.GetDriver()).SendKeys(charInUserName.ToString()).Perform();
                TimeWaiting.WaitSeconds(1);
            }
            new Actions(_browser.GetDriver()).SendKeys(Keys.Down).Perform();
            TimeWaiting.WaitSeconds(1);
            new Actions(_browser.GetDriver()).SendKeys(Keys.Enter).Perform();
            TimeWaiting.WaitSeconds(1);
            new Actions(_browser.GetDriver()).SendKeys(Keys.Tab).Perform();
            TimeWaiting.WaitSeconds(1);
            new Actions(_browser.GetDriver()).SendKeys(p1.ToString().Replace(".", ",")).Perform();
            TimeWaiting.WaitSeconds(1);
            new Actions(_browser.GetDriver()).SendKeys(Keys.Tab).Perform();
            TimeWaiting.WaitSeconds(1);

            const string linkText = "Commit";
            _linkCliker.ClickOnLinkWithText(linkText);
            TimeWaiting.WaitSeconds(5);
        }

        [Then(@"Check Balance added (.*)")]
        public void ThenCheckBalanceAdded(Decimal p0)
        {
            var currentBalance = GetBalance();
            Assert.AreEqual(p0, currentBalance - PreviousBalance, "Not expected balance value.");
        }

        [Then(@"Can not transfer money to yourself")]
        public void ThenCanNotTransferMoneyToYourself()
        {
            var expectedAlertText = "You can not transfer money to yourself";
            var alertText = _browser.GetDriver().SwitchTo().Alert().Text;
            _browser.GetDriver().SwitchTo().Alert().Dismiss();
            Assert.IsTrue(alertText.Contains(expectedAlertText), $"Not found expected alert with text [{expectedAlertText}].");
            
        }

        [When(@"Navigate to create")]
        public void WhenNavigateToCreate()
        {
            GivenNavigateToCreate();
        }

        [Then(@"No money")]
        public void ThenNoMoney()
        {
            var expectedPageText = "Can't creat transaction. Not enough funds!";
            var headerTag = _browser.GetDriver().FindElement(By.TagName("h2"));
            Assert.IsNotNull(headerTag, $"Can't find header tag in page.");
            
            Assert.AreEqual(expectedPageText, headerTag.Text, $"Not expected text on page. Expected header not found.");
        }

        [Then(@"Check Balance less (.*)")]
        public void ThenCheckBalanceLess(int p0)
        {
            var currentBalance = GetBalance();
            Assert.AreEqual(p0, PreviousBalance - currentBalance , "Not expected balance value.");
        }

        private void SetTextInTextBoxWithId(string id, string text)
        {
            var passwordElement = _browser.GetDriver().FindElement(By.Id(id));
            Assert.IsNotNull(passwordElement, $"Can't find [{id}] at the page.");
            passwordElement.Click();
            passwordElement.Clear();
            passwordElement.SendKeys(text);
        }

        private decimal GetBalance()
        {
            const string ballanceMessage = "Balance:";
            IList<IWebElement> links = _browser.GetDriver().FindElements(By.TagName("a"));
            var linkWithBalance = links.SingleOrDefault(element => element.Text.Contains(ballanceMessage));

            Assert.IsNotNull(linkWithBalance, $"Can't find [{ballanceMessage}] link in page.");

            var balanceValue = linkWithBalance.Text
                .Replace("Balance: ", string.Empty)
                .Replace(" PW", string.Empty)
                .Replace(",", ".");

            return Decimal.Parse(balanceValue);
        }
    }
}
