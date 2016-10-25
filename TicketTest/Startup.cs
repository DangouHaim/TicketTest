using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TicketTest.Startup))]
namespace TicketTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
