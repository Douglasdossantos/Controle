using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Controle.Startup))]
namespace Controle
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
