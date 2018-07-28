using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using VTechClubApp.Models;
using VTechClubApp.DAL;
namespace VTechClubApp
{
    public partial class Startup
    {
        VTechContext vtc = new VTechContext();
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
               app.CreatePerOwinContext(ApplicationDbContext.Create);

           //var cc = vtc.constants.

           // if (vtc.constants  )
           // constants rr = new constants(){RegistrationStatus=true } 





            




           /* app.CreatePerOwinContext(VTechContext.Create);*/

               app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
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
                    OnValidateIdentity = SecurityStampValidator
                 .OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                     validateInterval: TimeSpan.FromMinutes(30),
                     regenerateIdentityCallback: (manager, user) =>
                         user.GenerateUserIdentityAsync(manager),
                     getUserIdCallback: (id) => int.Parse(id.GetUserId()))
                }
            });            
           
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            app.UseMicrosoftAccountAuthentication(
                clientId: "00000000401D06AF",
                clientSecret: "94nYBkTwDXha9DmckusD9p6");
            ///twitter authetication doesn't work now because in the app settings it requires a complete domain name and not localhost
            app.UseTwitterAuthentication(
               consumerKey: "1479537228-OvZEs39V7oT1QLEvfaPGJA7XXXalUc8sgNNfgqh",
               consumerSecret: "XOBzTaCBx7S6XOaDMSRfrp1QZzUFA7YCVdQTIVtCruyRr");
          
            app.UseFacebookAuthentication(
               appId: "1123480867760977",
               appSecret: "bccd7e9ad195af2a85eec77cc454e05d");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "729483825672-j1a1qrpmaq7c0oetil6hmupl3aubjnl3.apps.googleusercontent.com",
                ClientSecret = "LoVWjv92hfJWPxrJ43fgqMk3"
            });
        }
    }
}