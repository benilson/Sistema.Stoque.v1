using Microsoft.Owin;
using Owin;
using Sistema.Stoque.v1.UI;

[assembly: OwinStartup(typeof(Startup))]
namespace Sistema.Stoque.v1.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
