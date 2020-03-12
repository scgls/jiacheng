using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web.WMS.Startup))]
namespace Web.WMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
