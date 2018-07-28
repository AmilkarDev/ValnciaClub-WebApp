using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using VTechClubApp.DAL;
using VTechClubApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.xml;
using System.Text;
using System.Xml;
using System.Collections;
namespace VTechClubApp.Controllers
{
    [RequireHttps]
    public class RegisterStudentController : Controller
    {
        private readonly VTechClubApp.DAL.VTechContext _db = new VTechContext();
        public int _cd { get; set; }
        public List<ToolTorepair> tools = new List<ToolTorepair>();

        public RegisterStudentController() { }


          public RegisterStudentController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }










        // GET: RegisterStudent
        //public ActionResult Create()
        //{
        //    ViewBag.EmailConfirmation = "Please make sure you register via a valid Email address , because you won't be able to login unless it's confirmed";
        //    Customer c = new Customer()
        //    {
        //        FirstName = "Holako",
        //        CustomerEmail = "hello@holako.com",
        //        CustomerPhone = "5555555555"
        //    };
        //    return View(c);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(Customer newCustomer)
        //{
            

        //    if (ModelState.IsValid)
        //    {
        //        _db.Customers.Add(newCustomer);
        //        _db.SaveChanges();
        //        Session["customer"] = newCustomer;
        //        _cd = newCustomer.CustomerId;

        //        return RedirectToAction("AddingUser");

        //       // return RedirectToAction("LaunchRepairCase");
        //    }
        //    else
        //    {
        //        return View(newCustomer);

        //    }
        //}
      



        //public JsonResult CheckForDuplication(string email)
        //{
        //    var data = UserManager.Users.Where(p => p.UserName.Equals(email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        //   // var data = _db.Customers.Where(p => p.CustomerEmail.Equals(email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        //    if (data != null)
        //    {
        //        return Json("Sorry, this Email address already exists", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //}
        [HttpGet]
        public ActionResult AddingUser()
        {
            ViewBag.EmailConfirmation = "Please make sure you register via a valid Email address , because you won't be able to login unless it's confirmed";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddingUser(RegisterViewModel user)
        {
            ViewBag.EmailConfirmation = "Please make sure you register via a valid Email address , because you won't be able to login unless it's confirmed";
            if (ModelState.IsValid)
            { 
                    var userr = new ApplicationUser
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email
                    };
                    var result = await UserManager.CreateAsync(userr, user.Password);
            
            if (result.Succeeded)
            {
                var resultt = await UserManager.AddToRoleAsync(userr.Id, "User");
                if (!resultt.Succeeded)
                {
                    ModelState.AddModelError("", resultt.Errors.First());
                    return View(user);
                }
                string callbackUrl = await SendEmailConfirmationTokenAsync(userr.Id, "Confirm your account");
                ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                            + "before you can log in.";
                //else
                //{
                //    Customer cuss = new Customer()
                //        {
                //            CustomerEmail=user.Email,
                //            FirstName=user.FirstName,
                //            LastName=user.LastName,
                //            CustomerPhone=user.PhoneNumber
                //        };
                //    var cust = _db.Customers.Where(x => x.CustomerEmail == cuss.CustomerEmail).ToList();
                //    Customer cus = new Customer();
                //    if (cust.Count == 0)
                //    {
                //        cus = new Customer
                //        {
                //           // CustomerId = user.Id ,
                //            CustomerEmail = user.Email,
                //            CustomerPhone = user.PhoneNumber,
                //            FirstName = user.FirstName,
                //            LastName = user.LastName,
                //            NumberOfRequests = 1
                //        };
                //        _db.Customers.Add(cus);
                //        _db.SaveChanges();
                //    }
                //    else
                //    {
                //        cust.FirstOrDefault().NumberOfRequests++;

                //        Customer cus1 = cust.FirstOrDefault();

                //        _db.SaveChanges();
                //        cus = new Customer
                //        {
                //            CustomerId = cus1.CustomerId,
                //            CustomerEmail = cus1.CustomerEmail,
                //            CustomerPhone = cus1.CustomerPhone,
                //            FirstName = cus1.FirstName,
                //            LastName = cus1.LastName,
                //            NumberOfRequests = cus1.NumberOfRequests
                //        };


                //    }
                //    Session["customer"] = cus;
                //    return RedirectToAction("LaunchRepairCase");
                //}
            }
            //else
            //{
            //    ModelState.AddModelError("", result.Errors.First());
            //    return View(user);
            
            //}
            }
           

            return View("Info");
            //return View();

        }
        public ActionResult LaunchRepairCase ()
        {
          //  List<Global> gl = new List<Global>();
            var gl = _db.Globals.ToList();
            if (gl.Count==0)
            {
                Global go = new Global()
                {
                    RegistrationStatus = true,
                    id = 1
                };
                _db.Globals.Add(go);
                _db.SaveChanges();
            }

             Global gg =  _db.Globals.FirstOrDefault();
            if(gg.RegistrationStatus==false)
            {
                return View("RegistrationDisabled");
            }
            tools = null;
            Session["toolsList"] = null;
            Customer cc = Session["customer"] as Customer;

            RepairCase rc = new RepairCase()
            {
                CustomerIdd = cc.CustomerId,
                ReceptionDate= DateTime.Today,
                RegistrationDate=DateTime.Today
            };
            
            return View(rc);
        }

        [HttpPost]
        public ActionResult LaunchRepairCase(RepairCase newRepairCase )
        {
            Customer cc = Session["customer"] as Customer;
            
            newRepairCase.CustomerIdd = cc.CustomerId;
            newRepairCase.RegistrationDate = DateTime.Today;
            if (ModelState.IsValid && newRepairCase.Agreement=="Agree" )
            {
                _db.RepairCases.Add(newRepairCase);
                _db.SaveChanges();
                Session["NewRpCase"] = newRepairCase ;
                return RedirectToAction("AddNewTool");
            }
            else
            {
                return View(newRepairCase);
            }
        }

        [HttpGet]
        public ActionResult AddNewTool()
        {

            var list = Session["toolsList"] as List<ToolTorepair>;
            
            RepairCase rr = Session["NewRpCase"] as RepairCase ;
            ViewBag.error1 = TempData["error1"] as string;
            ViewBag.error2 = TempData["error2"] as string;
            ViewBag.error3 = TempData["error3"] as string;
             IEnumerable<SelectListItem> ObjItem = new List<SelectListItem>()  
            {  
             new SelectListItem {Text="Select",Value="",Selected=true },  
                new SelectListItem {Text="Battery",Value="Battery" },  
                new SelectListItem {Text="Mouse",Value="Mouse"},  
                new SelectListItem {Text="Keyboard",Value="Keyboard"},  
                new SelectListItem {Text="Tower",Value="Tower" },  
                new SelectListItem {Text="Power Cord",Value="Power Cord" },  
                new SelectListItem {Text="Power Supply",Value="Power Supply" },  
                new SelectListItem {Text="Laptop",Value="Laptop" },
                 new SelectListItem {Text="Other",Value="Other" },
            };

             IEnumerable<SelectListItem> ObjItem1 = new List<SelectListItem>()  
            {  
               new SelectListItem {Text="Select",Value="",Selected=true },  
                new SelectListItem {Text="Black/Blue Screen",Value="Black/Blue Screen"},  
                new SelectListItem {Text="May Have a Virus",Value="May Have a Virus" },  
                new SelectListItem {Text="Won't Turn On",Value="Won't Turn On"},  
                new SelectListItem {Text="Install Software",Value="Install Software"},  
                new SelectListItem {Text="Repair Screen",Value="Repair Screen" },  
                new SelectListItem {Text="Repalce Hardware",Value="Repalce Hardware" },  
                new SelectListItem {Text="Remove Malware",Value="Remove Malware" },  
                new SelectListItem {Text="Dust Cleaning",Value="Dust Cleaning" },
                 new SelectListItem {Text="Other",Value="Other" },
            };

             IEnumerable<SelectListItem> ObjItem2 = new List<SelectListItem>()  
            {  
              new SelectListItem {Text="Select",Value="",Selected=true  },  
                new SelectListItem {Text="Dell",Value="Dell" },  
                new SelectListItem {Text="Mac",Value="Mac"},  
                new SelectListItem {Text="Toshiba",Value="Toshiba"},  
                new SelectListItem {Text="Acer",Value="Acer" },  
                new SelectListItem {Text="Lenovo",Value="Lenovo" },  
                new SelectListItem {Text="HP",Value="HP" },  
                new SelectListItem {Text="Sony",Value="Sony" },
                 new SelectListItem {Text="Fujistu",Value="Fujistu" },
                 new SelectListItem {Text="Asus",Value="Asus" },
                 new SelectListItem {Text="Samsung",Value="Samsung" },
                 new SelectListItem {Text="Other",Value="Other" },
            };

            ViewBag.ListItem3 = ObjItem;
            ViewBag.ListItem1 = ObjItem1;
            ViewBag.ListItem2 = ObjItem2;
            ToolTorepair tt = new ToolTorepair();
            return View(tt);
        }

        [HttpPost]
        public  ActionResult AddNewTool(ToolTorepair newTool, FormCollection form)
        {
            int id = Convert.ToInt32(Request["MoveType"]);
            RepairCase rr = Session["NewRpCase"] as RepairCase;
            newTool.RepairCaseIdd = rr.RepairCaseId;

            string str = form["ListItem3"].ToString();
            string str1 = form["ListItem1"].ToString();
            string str2 = form["ListItem2"].ToString();
            newTool.Type = str;
            newTool.Issue = str1;
            newTool.Manufacturer = str2;
            
            if ((str == "") || (str1 == "") || (str2 == ""))
            {
                //if (str == "")
                //{
                    TempData["error1"] = "please make sure you select a tool type ";
                //}
                //if (str1 == "")
                //{
                    TempData["error2"] = "please make sure you select a tool issue ,If you don't know it select other";
                //}
                //if (str2 == "")
                //{
                    TempData["error3"] = "please make sure you select a tool maufacturer,If you don't know it select other ";
                //}
                if (id == 0)
                {

                    if (Session["toolsList"] == null)
                    {
                        return RedirectToAction("AddNewTool",newTool);

                    }
                    return RedirectToAction("Choice");
                }
                return RedirectToAction("AddNewTool",newTool);
               
            }
          
            string ch="" ;
            //newTool.Type = str;
            //newTool.Issue = str1;
            //newTool.Manufacturer = str2;
            if (ModelState.IsValid)
            {
                tools.Add(newTool);
                if (Session["toolsList"] != null)
                {
                    List<ToolTorepair> list = Session["toolsList"] as List<ToolTorepair>;
                    list.Add(newTool);
                    Session["toolsList"] = list;
                }
                else
                    Session["toolsList"] = tools;





                switch (id)
                {
                    case 0:
                        _db.ToolsToRepair.Add(newTool);
                        _db.SaveChanges();
                        if(tools.Count!=0)
                        ch = "Choice";
                        else
                            ch = "AddNewTool";
                        
                        break;
                    case 1:
                        _db.ToolsToRepair.Add(newTool);
                        _db.SaveChanges();
                        ch = "AddNewTool";
                        
                        break;

                }

                return RedirectToAction(ch);
            }
            else
            {
                return View(newTool);
            }
        }

        public ActionResult Cancel()
        {
            RepairCase rr = Session["NewRpCase"] as RepairCase;
            Customer cc = Session["customer"] as Customer;
            if (rr != null)
            {
                var list1 = _db.ToolsToRepair.Where(x => x.RepairCaseIdd == rr.RepairCaseId).ToList();
                if (list1 != null)
                {
                    foreach (var item in list1)
                    {
                        _db.ToolsToRepair.Remove(item);
                    }
                }
                RepairCase rrc = _db.RepairCases.Find(rr.RepairCaseId);
                _db.RepairCases.Remove(rrc);
                _db.SaveChanges();
            }
            if ((cc != null)&&(rr!=null))
            {
                Customer cus = _db.Customers.Where(x => x.CustomerId == cc.CustomerId).FirstOrDefault();
                if (cus.NumberOfRequests > 1)
                    cus.NumberOfRequests--;
                else
                    _db.Customers.Remove(cus);
            }

            return RedirectToAction("Index","reception");
        }
        public ActionResult Choice()
        {
            

            RepairCase rr = Session["NewRpCase"] as RepairCase;
            Customer cc = Session["customer"] as Customer;



            string FirstName = cc.FirstName.ToString();
            string LastName = cc.LastName.ToString();
            string PhoneNumber = cc.CustomerPhone.ToString();
            string email = cc.CustomerEmail.ToString();
            string registerDate = DateTime.Today.Date.ToString() ;
            string receptiondate = rr.ReceptionDate.Date.ToString()  ;

            TempData["Name"] = FirstName;
            TempData["Pname"] = LastName;
            TempData["Phone"] = PhoneNumber;
            TempData["email"] = email;
            TempData["registerDate"] = registerDate;
            TempData["receptiondate"] = receptiondate;
            tools = Session["toolsList"] as List<ToolTorepair>;
            return View(tools);
        }
        
        //    public ActionResult Download()
        //{
        //    RepairCase rr = Session["NewRpCase"] as RepairCase;
        //    Customer cc = Session["customer"] as Customer;



        //    string FirstName = cc.FirstName.ToString();
        //    string LastName = cc.LastName.ToString();
        //    string PhoneNumber = cc.CustomerPhone;
        //    string email = cc.CustomerEmail.ToString();
        //    string registerDate = DateTime.Now.Date.ToString();
        //    string receptiondate = rr.ReceptionDate.ToString();

        //    TempData["Name"] = FirstName;
        //    TempData["Pname"] = LastName;
        //    TempData["Phone"] = PhoneNumber;
        //    TempData["email"] = email;
        //    TempData["registerDate"] = registerDate;
        //    TempData["receptiondate"] = receptiondate;
           
        //   tools = Session["toolsList"] as List<ToolTorepair>;
        //   return new ViewAsPdf("Choice", tools) { FileName = "Customer Invoice.pdf"  };
        //}


            //public ActionResult Print()
            //{
            //    RepairCase rr = Session["NewRpCase"] as RepairCase;
            //    Customer cc = Session["customer"] as Customer;
            //    string FirstName = cc.FirstName.ToString();
            //    string LastName = cc.LastName.ToString();
            //    string PhoneNumber = cc.CustomerPhone;
            //    string email = cc.CustomerEmail.ToString();
            //    string registerDate = DateTime.Now.Date.ToString();
            //    string receptiondate = rr.ReceptionDate.ToString();

            //    TempData["Name"] = FirstName;
            //    TempData["email"] = email;
            //    TempData["Pname"] = LastName;
            //    TempData["Phone"] = PhoneNumber;
            //    TempData["registerDate"] = registerDate;
            //    TempData["receptiondate"] = receptiondate;

            //    tools = Session["toolsList"] as List<ToolTorepair>;

            //    return ViewPdf(tools);

            //    // return new RazorPDF.PdfResult(tools,"Choice");
            //    //  return new ViewAsPdf("Choice", tools) ;
            //    return new ViewAsPdf("Choice", tools)
            //    {
            //        FileName = Server.MapPath("~/Content/CustomerInvoice.pdf"),
                    

            //    };

            //}


        public ActionResult print()
            {
                tools = Session["toolsList"] as List<ToolTorepair>;
                return ViewPdf(tools);
            }
        public ActionResult download ()
        {
            tools = Session["toolsList"] as List<ToolTorepair>;
            return ViewPdff(tools);
        }

            public byte[] GetPDF(string pHTML)
            {
                byte[] bPDF = null;

                MemoryStream ms = new MemoryStream();
                TextReader txtReader = new StringReader(pHTML);

                // 1: create object of a itextsharp document class
                Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

                // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
                PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

                // 3: we create a worker parse the document
                HTMLWorker htmlWorker = new HTMLWorker(doc);

                // 4: we open document and start the worker on the document
                doc.Open();
               
                htmlWorker.StartDocument();

                // 5: parse the html into the document
                htmlWorker.Parse(txtReader);

                // 6: close the document and the worker
                htmlWorker.EndDocument();
                htmlWorker.Close();
                doc.Close();

                bPDF = ms.ToArray();

                return bPDF;
            }

            public void DownloadPDF()
            {
              //  string HTMLContent = "Hello <b>World</b>";
                string HTMLContent = Server.MapPath("~/Content/CustomerInvoice.pdf").ToString();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(GetPDF(HTMLContent));
                Response.End();
            }












































            protected string RenderActionResultToString(ActionResult result)
            {
                // Create memory writer.
                var sb = new StringBuilder();
                var memWriter = new StringWriter(sb);

                // Create fake http context to render the view.
                var fakeResponse = new HttpResponse(memWriter);
                var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request,
                    fakeResponse);
                var fakeControllerContext = new ControllerContext(
                    new HttpContextWrapper(fakeContext),
                    this.ControllerContext.RouteData,
                    this.ControllerContext.Controller);
                var oldContext = System.Web.HttpContext.Current;
                System.Web.HttpContext.Current = fakeContext;

                // Render the view.
                result.ExecuteResult(fakeControllerContext);

                // Restore old context.
                System.Web.HttpContext.Current = oldContext;

                // Flush memory and return output.
                memWriter.Flush();
                return sb.ToString();
            }





            protected ActionResult  ViewPdf(object model)
            {
                // Create the iTextSharp document.
             //   Rectangle r = new Rectangle(700f, 700f);
                Document doc = new Document();
                //r.BackgroundColor = new CMYKColor(25, 90, 25, 0);
                //r.BackgroundColor = new Color(191, 64, 124);
                // Set the document to write to memory.
                MemoryStream memStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
                writer.CloseStream = false;
                doc.Open();
                string xmltext = this.RenderActionResultToString(this.View(tools));
                string xmltry = this.RenderActionResultToString(this.View("choice"));                
                XmlDocument xmldoc = new XmlDocument();
                ITextHandler textHandler = new ITextHandler(doc);
                string imagepath = Server.MapPath("~/Content/Images");
                doc.Add(new Paragraph("Valencia Technology Club                                                                         Computer Repair Branch"));
              //  doc.Add(new Paragraph("Valencia Technology Club                                                                    Computer Repair Branch"));
                Image gif = Image.GetInstance(imagepath + "/VtechLogo.png");
                gif.Alignment = Image.ALIGN_CENTER;
                doc.Add(gif);
                RepairCase rr = Session["NewRpCase"] as RepairCase;
                Customer cc = Session["customer"] as Customer;
                string FirstName = cc.FirstName.ToString();
                string LastName = cc.LastName.ToString();
                string PhoneNumber = cc.CustomerPhone;
                string email = cc.CustomerEmail.ToString();
                string registerDate = DateTime.Now.Date.ToString();
                string receptiondate = rr.ReceptionDate.ToString();

                Font lightblue = new Font(Font.COURIER, 20f, Font.NORMAL, new Color(43, 145, 175));
                Font georgia = FontFactory.GetFont("georgia", 15f);
                Font courier = new Font(Font.COURIER, 15f);
                georgia.Color = Color.GRAY;
                Chunk c1 = new Chunk("******* Final Customer Invoice *******\n", lightblue);
                Chunk c17 = new Chunk("\n\n");
                Chunk c2 = new Chunk("First Name            :", courier);
                Chunk c3 = new Chunk(FirstName +"\n", georgia);
                
                Chunk c4 = new Chunk("Last Name             :", courier);
                Chunk c5 = new Chunk(LastName + "\n", georgia);
                Chunk c6 = new Chunk("Phone Number          :", courier);
                Chunk c7 = new Chunk(PhoneNumber + "\n", georgia);
                Chunk c8 = new Chunk("Email                 :", courier);
                Chunk c9 = new Chunk(email + "\n", georgia);
                Chunk c10 = new Chunk("Registration Date     :", courier);
                Chunk c11 = new Chunk(registerDate + "\n", georgia);
                Chunk c12 = new Chunk("Reception Date        :", courier);
                Chunk c13= new Chunk(receptiondate + "\n", georgia);
                Chunk c14 = new Chunk("\n\n");
                Chunk c15 = new Chunk("***************  Tools  ***************", lightblue);
                Chunk c16 = new Chunk("\n\n");
                Phrase p12 = new Phrase();
                p12.Add(c1);
                p12.Add(c17);
                p12.Add(c2);
                p12.Add(c3);
                p12.Add(c4);
                p12.Add(c5);
                p12.Add(c6);
                p12.Add(c7);
                p12.Add(c8);
                p12.Add(c9);
                p12.Add(c10);
                p12.Add(c11);
                p12.Add(c12);
                p12.Add(c13);
                p12.Add(c14);
                p12.Add(c15);
                p12.Add(c16);
                Paragraph p = new Paragraph();
                p.Add(p12);
                doc.Add(p);

                TempData["Name"] = FirstName;
                TempData["email"] = email;
                TempData["Pname"] = LastName;
                TempData["Phone"] = PhoneNumber;
                TempData["registerDate"] = registerDate;
                TempData["receptiondate"] = receptiondate;

                Font link = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, new Color(0, 0, 255));
                Anchor anchor = new Anchor("https://vtech-computerrepairbranch.azurewebsites.net \n", link);
                Anchor anchor1 = new Anchor("http://valenciatechclub.com/  \n", link);
                anchor.Reference = "https://vtech-computerrepairbranch.azurewebsites.net";
                anchor1.Reference = "http://valenciatechclub.com/";
                    PdfPTable table = new PdfPTable(4);
                    PdfPCell cell = new PdfPCell(new Phrase( "Description of the repair case : "+rr.Note));
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                    table.AddCell("Description");
                    table.AddCell("type");
                    table.AddCell("Manufacturer");
                    table.AddCell("Issue");
                foreach( var element in tools)
                {
                    table.AddCell(element.description);
                    table.AddCell(element.Type);
                    table.AddCell(element.Manufacturer);
                    table.AddCell(element.Issue);
                }
                    doc.Add(table);
                    Chunk c18 = new Chunk("\n\n");
                    Phrase pp = new Phrase();
                    pp.Add(c18);
                   


                    Chunk c20 = new Chunk("For more information about Valencia Technology Club : \n");
                    Chunk c19 = new Chunk("To follow up your repair case : \n");
                    Anchor anchor2 = new Anchor(" https://vtech-computerrepairbranch.azurewebsites.net/Reception/MyRepairCases \n", link);
                    anchor2.Reference = " https://vtech-computerrepairbranch.azurewebsites.net/Reception/MyRepairCases";
                    pp.Add(c20);
                    pp.Add(anchor1);
                    pp.Add(c19);
                    pp.Add(anchor2);

                    Paragraph pr = new Paragraph();
                    pr.Add(pp);
                    doc.Add(pr);
                    //doc.Add(anchor);
                    //doc.Add(anchor1);
                //    Close your PDF
                doc.Close();
                    Response.ContentType = "application/pdf";
                //    Response.AddHeader("content-disposition", "attachment;filename=" + "FinalCustomerInvoice.pdf");
                byte[] buf = new byte[memStream.Position];
                memStream.Position = 0;
                memStream.Read(buf, 0, buf.Length);
                
                // Send the binary data to the browser.
                return new BinaryContentResult(buf, "application/pdf");
            }
            protected ActionResult ViewPdff(object model)
            {
                // Create the iTextSharp document.
                //   Rectangle r = new Rectangle(700f, 700f);
                Document doc = new Document();
                //r.BackgroundColor = new CMYKColor(25, 90, 25, 0);
                //r.BackgroundColor = new Color(191, 64, 124);
                // Set the document to write to memory.
                MemoryStream memStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, memStream);
                writer.CloseStream = false;
                doc.Open();
                string xmltext = this.RenderActionResultToString(this.View(tools));
                string xmltry = this.RenderActionResultToString(this.View("choice"));
                XmlDocument xmldoc = new XmlDocument();
                ITextHandler textHandler = new ITextHandler(doc);
                string imagepath = Server.MapPath("~/Content/Images");
               // doc.Add(new Paragraph("Valencia Technology Club                                                                    Computer Repair Branch"));
                doc.Add(new Paragraph("Valencia Technology Club                                                                         Computer Repair Branch"));
             
                //  doc.Add(new Paragraph("Valencia Technology Club                                                                    Computer Repair Branch"));
                Image gif = Image.GetInstance(imagepath + "/VtechLogo.png");
                gif.Alignment = Image.ALIGN_CENTER;
                doc.Add(gif);
                RepairCase rr = Session["NewRpCase"] as RepairCase;
                Customer cc = Session["customer"] as Customer;
                string FirstName = cc.FirstName.ToString();
                string LastName = cc.LastName.ToString();
                string PhoneNumber = cc.CustomerPhone;
                string email = cc.CustomerEmail.ToString();
                string registerDate = DateTime.Now.Date.ToString();
                string receptiondate = rr.ReceptionDate.ToString();

                Font lightblue = new Font(Font.COURIER, 20f, Font.NORMAL, new Color(43, 145, 175));
                Font georgia = FontFactory.GetFont("georgia", 15f);
                Font courier = new Font(Font.COURIER, 15f);
                georgia.Color = Color.GRAY;
                Chunk c1 = new Chunk("******* Final Customer Invoice *******\n", lightblue);
                Chunk c17 = new Chunk("\n\n");
                Chunk c2 = new Chunk("First Name            :", courier);
                Chunk c3 = new Chunk(FirstName + "\n", georgia);

                Chunk c4 = new Chunk("Last Name             :", courier);
                Chunk c5 = new Chunk(LastName + "\n", georgia);
                Chunk c6 = new Chunk("Phone Number          :", courier);
                Chunk c7 = new Chunk(PhoneNumber + "\n", georgia);
                Chunk c8 = new Chunk("Email                 :", courier);
                Chunk c9 = new Chunk(email + "\n", georgia);
                Chunk c10 = new Chunk("Registration Date     :", courier);
                Chunk c11 = new Chunk(registerDate + "\n", georgia);
                Chunk c12 = new Chunk("Reception Date        :", courier);
                Chunk c13 = new Chunk(receptiondate + "\n", georgia);
                Chunk c14 = new Chunk("\n\n");
                Chunk c15 = new Chunk("***************  Tools  ***************", lightblue);
                Chunk c16 = new Chunk("\n\n");
                Phrase p12 = new Phrase();
                p12.Add(c1);
                p12.Add(c17);
                p12.Add(c2);
                p12.Add(c3);
                p12.Add(c4);
                p12.Add(c5);
                p12.Add(c6);
                p12.Add(c7);
                p12.Add(c8);
                p12.Add(c9);
                p12.Add(c10);
                p12.Add(c11);
                p12.Add(c12);
                p12.Add(c13);
                p12.Add(c14);
                p12.Add(c15);
                p12.Add(c16);
                Paragraph p = new Paragraph();
                p.Add(p12);
                doc.Add(p);

                TempData["Name"] = FirstName;
                TempData["email"] = email;
                TempData["Pname"] = LastName;
                TempData["Phone"] = PhoneNumber;
                TempData["registerDate"] = registerDate;
                TempData["receptiondate"] = receptiondate;

                Font link = FontFactory.GetFont("Arial", 12, Font.UNDERLINE, new Color(0, 0, 255));
                Anchor anchor = new Anchor("https://vtech-computerrepairbranch.azurewebsites.net \n", link);
                Anchor anchor1 = new Anchor("http://valenciatechclub.com/  \n", link);
                anchor.Reference = "https://vtech-computerrepairbranch.azurewebsites.net";
                anchor1.Reference = "http://valenciatechclub.com/";
                PdfPTable table = new PdfPTable(4);
                PdfPCell cell = new PdfPCell(new Phrase("Description of the repair case : " + rr.Note));
                cell.Colspan = 4;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell("Description");
                table.AddCell("type");
                table.AddCell("Manufacturer");
                table.AddCell("Issue");
                foreach (var element in tools)
                {
                    table.AddCell(element.description);
                    table.AddCell(element.Type);
                    table.AddCell(element.Manufacturer);
                    table.AddCell(element.Issue);
                }
                doc.Add(table);
                Chunk c18 = new Chunk("\n\n");
                Phrase pp = new Phrase();
                pp.Add(c18);



                Chunk c20 = new Chunk("For more information about Valencia Technology Club : \n");
                Chunk c19 = new Chunk("To follow up your repair case : \n");
                Anchor anchor2 = new Anchor(" https://vtech-computerrepairbranch.azurewebsites.net/Reception/MyRepairCases \n", link);
                anchor2.Reference = " https://vtech-computerrepairbranch.azurewebsites.net/Reception/MyRepairCases";
                pp.Add(c20);
                pp.Add(anchor1);
                pp.Add(c19);
                pp.Add(anchor2);

                Paragraph pr = new Paragraph();
                pr.Add(pp);
                doc.Add(pr);
                //doc.Add(anchor);
                //doc.Add(anchor1);
                //    Close your PDF
                doc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "FinalCustomerInvoice.pdf");
               Response.Cache.SetCacheability(HttpCacheability.NoCache);
                byte[] buf = new byte[memStream.Position];
                memStream.Position = 0;
                memStream.Read(buf, 0, buf.Length);

                // Send the binary data to the browser.
                return new BinaryContentResult(buf, "application/pdf");
            }

            public class BinaryContentResult : ActionResult
            {
                private string ContentType;
                private byte[] ContentBytes;

                public BinaryContentResult(byte[] contentBytes, string contentType)
                {
                    this.ContentBytes = contentBytes;
                    this.ContentType = contentType;
                }

                public override void ExecuteResult(ControllerContext context)
                {
                    var response = context.HttpContext.Response;
                    response.Clear();
                    response.Cache.SetCacheability(HttpCacheability.NoCache);
                    response.ContentType = this.ContentType;

                    var stream = new MemoryStream(this.ContentBytes);
                    stream.WriteTo(response.OutputStream);
                    stream.Dispose();
                }
            }
















            private async Task<string> SendEmailConfirmationTokenAsync(int userID, string subject)
            {
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
                var callbackUrl = Url.Action("ConfirmEmail", "Account",
                   new { userId = userID, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(userID, subject,
                   "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return callbackUrl;
            }




    }
}