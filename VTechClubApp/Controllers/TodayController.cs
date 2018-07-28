using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTechClubApp.Models;
using VTechClubApp.DAL;

namespace VTechClubApp.Controllers
{
    public class TodayController : Controller
    {
        // GET: Today
        VTechContext vtc = new VTechContext();
        public ActionResult Index()
        {
            var repcse = vtc.RepairCases.Where(x => (x.ReceptionDate == DateTime.Today) || (x.PickUpDate == DateTime.Today) || (x.LatestPickUpDate == DateTime.Today));
            
            return View(repcse);
        }

        public ActionResult ReceivedToday()
        {
            var repcse = vtc.RepairCases.Where(x => x.ReceptionDate == DateTime.Today);
            return View(repcse);
        }

        public ActionResult PickupToday()
        {
            var repcse = vtc.RepairCases.Where(x => x.PickUpDate == DateTime.Today);
            return View(repcse);
        }

        public ActionResult LatestPickupToday()
        {
            var repcse = vtc.RepairCases.Where(x => x.LatestPickUpDate == DateTime.Today);
            return View(repcse);
        }



        // GET: Today/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //// GET: Today/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Today/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Today/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Today/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Today/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Today/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
