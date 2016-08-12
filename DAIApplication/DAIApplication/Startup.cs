using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DAIApplication.Startup))]
namespace DAIApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
