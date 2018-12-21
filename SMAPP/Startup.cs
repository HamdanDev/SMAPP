using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMAPP.Startup))]
namespace SMAPP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
