using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Basecamp.Startup))]
namespace Basecamp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
