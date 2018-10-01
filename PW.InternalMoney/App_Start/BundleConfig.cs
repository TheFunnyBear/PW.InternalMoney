using System.Web;
using System.Web.Optimization;

namespace PW.InternalMoney
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.min.js",
                        "~/Scripts/angular-route.min.js",
                        "~/Scripts/angucomplete-alt.js",
                        "~/Scripts/transactionAmount.js",
                        "~/Script/ngCurrency/ng-currency-settings.provider.js",
                        "~/Script/ngCurrency/ng-currency.directive.js",
                        "~/Script/ngCurrency/ng-currency.module.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                       "~/Scripts/bootstrap.min.js",
                       "~/Scripts/moment.min.js",
                       "~/Scripts/bootstrap-sortable.js",
                       "~/Scripts/filterCorrespondents.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-sortable.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/angular").Include(
                      "~/Content/angucomplete-alt.css"));
        }
    }
}
