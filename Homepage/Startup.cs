using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Homepage.Startup))]
namespace Homepage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
