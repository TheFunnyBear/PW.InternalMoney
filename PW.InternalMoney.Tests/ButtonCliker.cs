using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PW.InternalMoney.Tests
{
    public sealed class ButtonCliker : IButtonCliker
    {
        private readonly IBrowserNavigator _browser;

        public ButtonCliker(IBrowserNavigator browser)
        {
            _browser = browser;
        }

        public void ClickOnButtonWithValue(string valueText)
        {
            IList<IWebElement> links = _browser.GetDriver().FindElements(By.ClassName("btn"));
            var creatMessageLink = links.SingleOrDefault(element => element.GetProperty("value").Equals(valueText, StringComparison.OrdinalIgnoreCase));
            if (creatMessageLink == null)
            {
                var existedLinks = string.Join(", ", links.Select(link => $"Tag: {link.TagName}, Value: {link.GetProperty("value")}"));
                Assert.Fail($"Can't find [{valueText}] button in page. Existed links is: [{existedLinks}]");
            }
            creatMessageLink.Click();
        }
    }
}
