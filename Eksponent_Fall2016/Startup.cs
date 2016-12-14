using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Eksponent_Fall2016.Startup))]
namespace Eksponent_Fall2016
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
