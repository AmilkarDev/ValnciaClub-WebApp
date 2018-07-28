using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using VTechClubApp.Models;
using VTechClubApp.DAL;
using System.Net.Mail;
using System.Text;
using System.Net;
namespace VTechClubApp.Controllers
{
    public class ReceptionController : Controller
    {

        VTechContext vtc = new VTechContext();
        public ReceptionController() { }
            public ReceptionController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            vtc = new VTechContext();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Reception
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel vm)
        {

            bool IsMessageSent = true;
            if (ModelState.IsValid)
            {

                //prepare email
                var toAddress = "melek.ferhi@gmail.com";
                var fromAddress = vm.email.ToString();
                var subject = "Contact enquiry from Vtech Repair Branch Web App by " + vm.name;
                var message = new StringBuilder();
                message.Append("Name: " + vm.name + "<br/>");
                message.Append("Email: " + vm.email + "<br/>");
                message.Append("Subject: " + vm.subject + "<br/> <br/>");
                message.Append(vm.Message);
                string mes = message.ToString();
                //start email Thread

                try
                {
                    var tEmail = new Thread(() =>
                   SendEmail(toAddress, fromAddress, subject, mes));
                    tEmail.Start();

                    return View("Thanks");
                }
                catch (Exception ex)
                {
                    return View("Sorry");
                }
            }
            else
            {
                IsMessageSent = false;
            }
           // return PartialView("_SubmitMessage", IsMessageSent);
            return View(vm);
        }
        public void SendEmail(string toAddress, string fromAddress,
                      string subject, string message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    //const string email = "username@yahoo.com";
                    //const string password = "password!";

                    var loginInfo = new NetworkCredential("MalekFerhi", "GoHell123!");


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (var smtpClient = new SmtpClient(
                                                         "smtp.sendgrid.net", 587))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                    }

                    finally
                    {
                        //dispose the client
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write("Failed to deliver message");
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
        public async Task<ActionResult> MyRepairCases()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
         string email = user.Email;
         var rc = vtc.RepairCases.Where(x => x.Customer.CustomerEmail == email).ToList();
         return View(rc);

        }
        public async Task<ActionResult> registeredRC()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            string email = user.Email;
            var rc = vtc.RepairCases.Where(x => (x.Customer.CustomerEmail == email) && (x.Situation == advancement.Registered)).ToList();
            return View(rc);

        }
        public async Task<ActionResult> InProgressRC()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            string email = user.Email;
            var rc = vtc.RepairCases.Where(x => (x.Customer.CustomerEmail == email)&&(x.Situation==advancement.InProgress)).ToList();
            return View(rc);

        }
        public async Task<ActionResult> FinishedRC()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            string email = user.Email;
            var rc = vtc.RepairCases.Where(x => (x.Customer.CustomerEmail == email) && (x.Situation == advancement.Finished)).ToList();
            return View(rc);

        }
        public ActionResult Tools(int id)
        {
            return View(vtc.ToolsToRepair.Where(t => t.RepairCaseIdd == id).ToList());

        }
        public async Task<ActionResult> NewRepairCase()
        {

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            var cust = vtc.Customers.Where(x => x.CustomerEmail == user.Email).ToList();
            Customer cus=new Customer();
            
            if (cust.Count == 0) 
            {
                cus = new Customer
                    {
                        CustomerId=user.Id  ,
                        CustomerEmail=user.Email,
                        CustomerPhone=user.PhoneNumber,
                        FirstName=user.FirstName,
                        LastName=user.LastName,
                        NumberOfRequests=1
                    };
                vtc.Customers.Add(cus); 
                vtc.SaveChanges();
            }
            else
            {
                cust.FirstOrDefault().NumberOfRequests++;

               Customer cus1 = cust.FirstOrDefault();
             
               vtc.SaveChanges();
                cus = new Customer
                {
                    CustomerId = cus1.CustomerId   ,
                    CustomerEmail = cus1.CustomerEmail ,
                    CustomerPhone = cus1.CustomerPhone,
                    FirstName = cus1.FirstName,
                    LastName = cus1.LastName,
                    NumberOfRequests = cus1.NumberOfRequests
                };
                
             
            }

            Session["customer"] = cus;
            return RedirectToAction("LaunchRepairCase", "RegisterStudent");
        }

       

    }
}