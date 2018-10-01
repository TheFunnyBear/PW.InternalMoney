using OpenQA.Selenium.Remote;

namespace PW.InternalMoney.Tests
{
    public interface IBrowserNavigator
    {
        void Open();
        void Close();
        void Navigate(string webSiteAddress);
        string GetTitle();
        RemoteWebDriver GetDriver();
    }
}
