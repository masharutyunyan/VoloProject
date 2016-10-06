using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BooksCatalogue.Startup))]
namespace BooksCatalogue
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
