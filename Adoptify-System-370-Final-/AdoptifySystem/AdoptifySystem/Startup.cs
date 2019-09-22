using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web.Security;

[assembly: OwinStartupAttribute(typeof(AdoptifySystem.Startup))]
namespace AdoptifySystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Admin/Login")
            });
            //ConfigureAuth(app);
        }
    }
}
