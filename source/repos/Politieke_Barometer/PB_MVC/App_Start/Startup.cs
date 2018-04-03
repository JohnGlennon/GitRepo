using System;
using Microsoft.AspNet.Identity;
using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using BL.IdentityFramework;
using Domain.IdentityFramework;

namespace PB_MVC.App_Start
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
    }
    // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
    public void ConfigureAuth(IAppBuilder app)
    {
      // Configure the db context, user manager and signin manager to use a single instance per request
      //app.CreatePerOwinContext(new PBDbContext());
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
      app.CreatePerOwinContext(ApplicationRoleManager.Create);
      app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

      // Enable the application to use a cookie to store information for the signed in user
      // and to use a cookie to temporarily store information about a user logging in with a third party login provider
      // Configure the sign in cookie
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString("/Account/Login"),
        Provider = new CookieAuthenticationProvider
        {
          // Enables the application to validate the security stamp when the user logs in.
          // This is a security feature which is used when you change a password or add an external login to your account.  
          OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                  validateInterval: TimeSpan.FromMinutes(30),
                  regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        }
      });
      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

      // Uncomment the following lines to enable logging in with third party login providers
      //app.UseMicrosoftAccountAuthentication(
      //    clientId: "",
      //    clientSecret: "");

      app.UseTwitterAuthentication(
         consumerKey: "Crk6TJxd5aFRSZ9NGNi1u9LVL",
         consumerSecret: "I7tSlcl4HOBnWzy6mlqObxKiZAsZc8NJ5dBl8f1FTAogbr3DX9");

      app.UseFacebookAuthentication(
         appId: "361839047668602",
         appSecret: "195d3b87dc01352d734dc61e6dd58437");

      app.UseGoogleAuthentication(
        clientId: "768694855243-f2tcmbc2msa2khpakoua77akij3afhf6.apps.googleusercontent.com",
        clientSecret: "2No0z4IaOltlwHpk4lyYdChh");
    }
  }
}