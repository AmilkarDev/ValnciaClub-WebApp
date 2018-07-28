using System;
using System.Data.Entity ;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTechClubApp.DAL;
using VTechClubApp.Models;
using System.Data.Entity.Core.Objects;

namespace VTechClubApp.Controllers
{

    //[RequireHttps]
   // [Authorize(Roles = "Master")]
    [AccessDeniedAuthorize(Roles = "Master")]
    public class HomeController : Controller
    {
       private readonly  VTechClubApp.DAL.VTechContext vctxt = new VTechContext(); 

       // [Authorize(Roles="Master")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            int id = Convert.ToInt32(Request["SearchType"]);
            //ViewBag.Message = "Your application description page.";

            //return View();

            dynamic mymodel = new ExpandoObject();


       

                    IQueryable<ReceptionDateGroup> data1 = from repcase in vctxt.RepairCases
                                                          group repcase by repcase.ReceptionDate into dateGroup
                                                          select new ReceptionDateGroup()
                                                          {
                                                              ReceptionDate = System.Data.Entity.DbFunctions.TruncateTime(dateGroup.Key),
                                                              RepairCaseCount = dateGroup.Count()
                                                          };
                    mymodel.data01 = data1;
                    


                

                        IQueryable<ManufacturerGroup> data2 = from Manufacturers_Tools in vctxt.ToolsToRepair
                                                             group Manufacturers_Tools by Manufacturers_Tools.Manufacturer into dateGroup
                                                             select new ManufacturerGroup()
                                                             {
                                                                 Manufacturer = dateGroup.Key,
                                                                 ManufacturerCount = dateGroup.Count()

                                                             };

                        mymodel.data02 = data2;
                 

                        IQueryable<IssuesGroup> data3 = from Issues in vctxt.ToolsToRepair
                                                       group Issues by Issues.Issue into dateGroup
                                                       select new IssuesGroup()
                                                       {
                                                           IssueName = dateGroup.Key,
                                                           Count = dateGroup.Count()

                                                       };



                        mymodel.data03 = data3;


                        return View(mymodel);




            
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult HandleRegistration()
        {
            var cc = vctxt.Globals.ToList();
            
            if (cc.Count == 0)
            {
                Global cc1 = new Global() { 
                    id=1,
                    RegistrationStatus = true };
                vctxt.Globals.Add(cc1);
                vctxt.SaveChanges();
            }
            
            ViewBag.activate = "Deactivate Registration";
            Global cs = vctxt.Globals.FirstOrDefault();
            if(cs.RegistrationStatus==true)
                ViewBag.activate = "Deactivate Registration";
            else
                ViewBag.activate = "Activate Registration";
            return View(cs);
          //  RedirectToAction("activateRegistration");
        }

        public ActionResult activateRegistration()
        {
            Global cs = vctxt.Globals.FirstOrDefault();
            if (cs.RegistrationStatus == true)
            {
                cs.RegistrationStatus = false;
                vctxt.SaveChanges();
                ViewBag.activate = "Activate Registration";
            }
            else
            {
                cs.RegistrationStatus = true;
                vctxt.SaveChanges();
                ViewBag.activate = "Deactivate Registration";
            }
            return View("HandleRegistration",cs);
        }

        protected override void Dispose(bool disposing)
        {
            vctxt.Dispose();
            base.Dispose(disposing);
        }

    }
}