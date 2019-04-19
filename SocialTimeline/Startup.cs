using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialTimeline.Startup))]
namespace SocialTimeline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          //  ConfigureAuth(app);
        }
    }
}
