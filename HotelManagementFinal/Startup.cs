using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotelManagementFinal.Startup))]
namespace HotelManagementFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
