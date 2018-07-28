using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VTechClubApp.Startup))]
namespace VTechClubApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
