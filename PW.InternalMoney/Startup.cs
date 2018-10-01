using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PW.InternalMoney.Startup))]
namespace PW.InternalMoney
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
