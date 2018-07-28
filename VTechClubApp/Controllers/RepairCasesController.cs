using PagedList;
using PagedList.Mvc;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VTechClubApp.DAL;
using VTechClubApp.Models;

namespace VTechClubApp.Controllers
{
    public class RepairCasesController : Controller
    {
        private VTechClubApp.DAL.VTechContext _db = new VTechContext();



        public ActionResult All()
        {
            Session["id"] = null; Session["param"] = null;
            Index(null, 1, 5);
            return View("index");
        }

        public ActionResult ContactCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairCase repairCase = _db.RepairCases.Find(id);
            if (repairCase == null)
            {
                return HttpNotFound();

            }


            Customer cus = repairCase.Customer;
            if (cus == null)
            {
                return HttpNotFound();

            }
            return View(cus);
        }
        // GET: RepairCases
        
       // [Authorize(Roles="Technician")  ]


        public ActionResult Index(string param, int page = 1, int pageSize = 5)
        {
            
           // var repcse = from p in _db.RepairCases select p;
            var repcse = _db.RepairCases.OrderByDescending(x=>x.ReceptionDate).ToList();
            PagedList<RepairCase> model = new PagedList<RepairCase>(repcse, page, pageSize);
            int id = Convert.ToInt32(Request["SearchType"]);
            var searchParameter = "Searching";
            if ((!string.IsNullOrWhiteSpace(param))||(Session["param"]!=null))
            {
                if (param == null)
                { param = Session["param"].ToString(); id = (int)Session["id"]; }

                    switch (id)
                    {
                        case 0:
                            int iQ = int.Parse(param);
                            repcse = _db.RepairCases.Where(x => x.RepairCaseId == iQ).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " Id for ' " + param + " '";
                            break;
                        case 1:

                            DateTime tt = Convert.ToDateTime(param);
                            repcse = _db.RepairCases.Where(y => y.ReceptionDate == tt).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " Reception Date ' " + param + " '";
                            break;
                        case 2:
                            DateTime tt2 = Convert.ToDateTime(param);
                            repcse = _db.RepairCases.Where(z => z.CompletionDate == tt2).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " Completion Date ' " + param + " '";
                            break;
                        case 3:
                            DateTime tt3 = Convert.ToDateTime(param);
                            repcse = _db.RepairCases.Where(h => h.PickUpDate == tt3).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " PickUp Date ' " + param + " '";
                            break;
                        case 4:
                            DateTime tt4 = Convert.ToDateTime(param);
                            repcse = _db.RepairCases.Where(i => i.LatestPickUpDate == tt4).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " Latest PickUp Date ' " + param + " '";
                            break;
                        case 5:
                            int iQ1 = int.Parse(param);
                            repcse = _db.RepairCases.Where(j => j.TechnicianIdd == iQ1).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " Technicians Id' " + param + " '";
                            break;
                        case 6:
                            int iQ2 = int.Parse(param);
                            repcse = _db.RepairCases.Where(k => k.CustomerIdd == iQ2).ToList();
                            model = new PagedList<RepairCase>(repcse, page, pageSize);
                            searchParameter += " Customer Id ' " + param + " '";
                            break;
                    }
                
            }
            else
            {
                searchParameter += "ALL";
            }
           
            ViewBag.SearchParameter = searchParameter;
            Session["param"] = param;
            Session["id"] = id;
            return View(model);
           // return View(repcse);

                
            
        }

        public ActionResult Registered( int page = 1, int pageSize = 5)
        {
            var repcse = _db.RepairCases.Where(x => x.Situation == advancement.Registered).OrderByDescending(x => x.ReceptionDate).ToList();
            PagedList<RepairCase> model = new PagedList<RepairCase>(repcse, page, pageSize);
            return View(model);
        }
        public ActionResult InProgress( int page = 1, int pageSize = 5)
        {
            var repcse = _db.RepairCases.Where(x => x.Situation == advancement.InProgress).OrderByDescending(x => x.ReceptionDate).ToList();
            PagedList<RepairCase> model = new PagedList<RepairCase>(repcse, page, pageSize);
            return View(model);
        }
        public ActionResult Finished( int page = 1, int pageSize = 5)
        {
            var repcse = _db.RepairCases.Where(x => x.Situation == advancement.Finished).OrderByDescending(x => x.ReceptionDate).ToList();
            PagedList<RepairCase> model = new PagedList<RepairCase>(repcse, page, pageSize);
            return View(model);
        }

        // GET: RepairCases/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairCase repairCase = _db.RepairCases.Find(id);
            if (repairCase == null)
            {
                return HttpNotFound();
                
            }
            return View(repairCase);
        }

        // GET: RepairCases/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RepairCases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RepairCaseId,Note,ReceptionDate,CompletionDate,PickUpDate,LatestPickUpDate,Agreement,TechnicianIdd,CustomerIdd")] RepairCase repairCase)
        {
            if (ModelState.IsValid)
            {
                repairCase.Situation = advancement.Registered;
                repairCase.RegistrationDate = DateTime.Today;
                _db.RepairCases.Add(repairCase);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(repairCase);
        }


        // GET: RepairCases/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairCase repairCase = _db.RepairCases.Find(id);
            if (repairCase == null)
            {
                return HttpNotFound();
            }
            
            return View(repairCase);
            
        }

        // POST: RepairCases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RepairCaseId,Note,TechNote,RegistrationDate,ReceptionDate,CompletionDate,PickUpDate,LatestPickUpDate,Agreement,TechnicianIdd,CustomerIdd")] RepairCase repairCase)
        {
            if (ModelState.IsValid)
            {
                if ((repairCase.TechnicianIdd != null)&&(repairCase.Situation==advancement.Registered) ) repairCase.Situation = advancement.InProgress;
                if (repairCase.CompletionDate != null) repairCase.Situation = advancement.Finished;
                repairCase.Agreement = "Agree";
                _db.Entry(repairCase).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(repairCase);
        }

        // GET: RepairCases/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairCase repairCase = _db.RepairCases.Find(id);
            if (repairCase == null)
            {
                return HttpNotFound();
            }
            return View(repairCase);
        }

        // POST: RepairCases/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RepairCase repairCase = _db.RepairCases.Find(id);
            var list = repairCase.Tools.ToList();
            var list1 = _db.ToolsToRepair.Where(x => x.RepairCaseIdd == repairCase.RepairCaseId);
            foreach (var item in list1)
            {
                _db.ToolsToRepair.Remove(item);
            }
            _db.RepairCases.Remove(repairCase);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Tools(int id)
        {
            return View(_db.ToolsToRepair.Where(t => t.RepairCaseIdd == id).ToList());

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
