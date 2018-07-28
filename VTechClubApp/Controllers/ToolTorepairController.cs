using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VTechClubApp.DAL;
using VTechClubApp.Models;

namespace VTechClubApp.Controllers
{
    public class ToolTorepairController : Controller
    {
        private readonly VTechContext db = new VTechContext();

        // GET: ToolTorepair
         [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
         
            List<ToolTorepair> toolsToRepair = db.ToolsToRepair.ToList();
            PagedList<ToolTorepair> model = new PagedList<ToolTorepair>(toolsToRepair, page, pageSize);
            return View(model);
        }

        // GET: ToolTorepair/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolTorepair toolTorepair = db.ToolsToRepair.Find(id);
            if (toolTorepair == null)
            {
                return HttpNotFound();
            }
            return View(toolTorepair);
        }

        // GET: ToolTorepair/Create
        public ActionResult Create()
        {
            ViewBag.RepairCaseIdd = new SelectList(db.RepairCases, "RepairCaseId", "Note");
            ViewBag.TechnicianId = new SelectList(db.Technicians, "TechnicianId", "FirstName");
            return View();
        }

        // POST: ToolTorepair/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToolToRepairId,Type,Manufacturer,Issue,Result,TechnicianId,RepairCaseIdd")] ToolTorepair toolTorepair)
        {
            if (ModelState.IsValid)
            {
                db.ToolsToRepair.Add(toolTorepair);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RepairCaseIdd = new SelectList(db.RepairCases, "RepairCaseId", "Note", toolTorepair.RepairCaseIdd);
            ViewBag.TechnicianId = new SelectList(db.Technicians, "TechnicianId", "FirstName", toolTorepair.TechnicianId);
            return View(toolTorepair);
        }

        // GET: ToolTorepair/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolTorepair toolTorepair = db.ToolsToRepair.Find(id);
            if (toolTorepair == null)
            {
                return HttpNotFound();
            }
            ViewBag.RepairCaseIdd = new SelectList(db.RepairCases, "RepairCaseId", "RepairCaseId", toolTorepair.RepairCaseIdd);
            ViewBag.TechnicianId = new SelectList(db.Technicians, "TechnicianId", "FirstName", toolTorepair.TechnicianId);
            return View(toolTorepair);
        }

        // POST: ToolTorepair/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToolToRepairId,Type,Manufacturer,Issue,Result,TechnicianId,RepairCaseIdd")] ToolTorepair toolTorepair)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toolTorepair).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RepairCaseIdd = new SelectList(db.RepairCases, "RepairCaseId", "RepairCaseId", toolTorepair.RepairCaseIdd);
            ViewBag.TechnicianId = new SelectList(db.Technicians, "TechnicianId", "FirstName", toolTorepair.TechnicianId);
            return View(toolTorepair);
        }

        // GET: ToolTorepair/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToolTorepair toolTorepair = db.ToolsToRepair.Find(id);
            if (toolTorepair == null)
            {
                return HttpNotFound();
            }
            return View(toolTorepair);
        }

        // POST: ToolTorepair/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToolTorepair toolTorepair = db.ToolsToRepair.Find(id);
            db.ToolsToRepair.Remove(toolTorepair);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
