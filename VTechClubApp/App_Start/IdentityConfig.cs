using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using VTechClubApp.Models;
using System.Diagnostics;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Clients;



namespace VTechClubApp
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
          
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
            await configSendGridasync(message);
        }
        private async Task configSendGridasync(IdentityMessage message)
        {
         //   var myMessage = new SendGridMessage();
         //   myMessage.AddTo(message.Destination);
         //   myMessage.From = new System.Net.Mail.MailAddress(
         //                       "melek.ferhi@gmail.com", "VTechClub-ComputerRepairBranch");
         //   myMessage.Subject = message.Subject;
         //   myMessage.Text = message.Body;
         //   myMessage.Html = message.Body;

            //var credentials = new NetworkCredential(
            //           ConfigurationManager.AppSettings["mailAccount"],
            //           ConfigurationManager.AppSettings["mailPassword"]
            //           );
         //   var cre = new NetworkCredential();
         //   cre.UserName = "apikey";
         //   cre.Password = "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";
           
         //   // Create a Web transport for sending email.
         ////  var transportWeb = new Web(credentials);
         // var transportWeb = new  Web(cre );
         //   // Send the email.
         //   if (transportWeb != null)
         //   {
         //       await transportWeb.DeliverAsync(myMessage);
         //   }
         //   else
         //   {
         //       Trace.TraceError("Failed to create Web transport.");
         //       await Task.FromResult(0);
         //   }



            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient("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");
            var from = new EmailAddress("melek.ferhi@gmail.com", "VTechClub-ComputerRepairBranch");
            var subject = message.Subject;
            var to = new EmailAddress(message.Destination);
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);


            //byte[] imageArray = System.IO.File.ReadAllBytes(@"~\Images\VtechLogo.png");
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            //var att = new Attachment()
            //{
            //    Content = Convert.ToBase64String(imageArray),
            //    Type="image/png",
            //    Filename="VtechLogo.png",
            //    ContentId="Vtech 2",
            //    Disposition="inline",
            //    };

            //msg.AddAttachment("VtechLogo.png", base64ImageRepresentation);
           msg.AddSubstitution("-name-", "VTech Club App");
            msg.AddSubstitution("-city-", "Orlando");
            msg.AddSubstitution("-state-", "Florida");
            msg.AddHeader("-header-", "Welcome to Vtech Club app");
            
            //msg.SetFooterSetting( 
            //         true,
            //         "hello world",
            //         "<br/><h1>hello world</h1>");
         // Attachment att = new Attachment{ s}



            //msg.SetFooterSetting(
            //          true,
            //          "Computer Repair Branch",
            //          "<strong>Valencia Technology Club</strong>");
            var response = await client.SendEmailAsync(msg);





        }
    }

    //public class SmsService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your SMS service here to send a text message.
    //       /* return Task.FromResult(0);*/
    //        // Twilio Begin
    //        /* var Twilio = new TwilioRestClient(
    //           Keys.SMSAccountIdentification,
    //           Keys.SMSAccountPassword);*/


    //        string AccountSid = "AC37a804308178c35a5ebf6357f2063dd4";
    //        string AuthToken = "08db88941b10a0988c6e6cf0e36f34a3";
    //         var twilio = new TwilioRestClient(AccountSid, AuthToken);

    //         var result = twilio.SendMessage(Keys.SMSAccountFrom,
    //           message.Destination, message.Body
    //         );

            


    //        // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
    //         Trace.TraceInformation(result.Status);
    //         //Twilio doesn't currently have an async API, so return success.
    //         return Task.FromResult(0);
    //        // Twilio End





    //    }
    //}

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser,int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser,int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
             var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ApplicationDbContext>()));

           
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser,int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser,int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser,int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser,int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }




    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<CustomRole,int>
    {
        public ApplicationRoleManager(IRoleStore<CustomRole, int> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new CustomRoleStore(context.Get<ApplicationDbContext>()));
        }
    }




    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
