using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StockSimulator.Startup))]
namespace StockSimulator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
