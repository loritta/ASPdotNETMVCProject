using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPdotNETMVCProject.Startup))]
namespace ASPdotNETMVCProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
