using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VTechClubApp.DAL;
using VTechClubApp.Models;
using Microsoft.AspNet.Identity.Owin;



namespace VTechClubApp.Controllers
{
    public class TechnicianController : Controller
    {
        private  readonly VTechClubApp.DAL.VTechContext _db ;
        private readonly ApplicationDbContext vtc;

        public TechnicianController()
        {
            vtc = new ApplicationDbContext();
            _db = new VTechContext();
        }


        public TechnicianController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        // GET: Technician
      //  [Authorize(Roles = "fg")]
        public ActionResult Index()
        {
            return View(_db.Technicians.ToList());
            ////List<ApplicationUser> xx = new List<ApplicationUser>();


            ////var users = UserManager.Users.ToList();
            ////foreach (var item in users)
            ////{
            ////    var userRoles = await UserManager.GetRolesAsync(item.Id);
            ////    if (userRoles.Contains("Technician")) xx.Add(item);

            ////}
            ////return View(xx);

            
        }
        //[Authorize(Roles="Technician")]
        //public ActionResult Print()
        //{
        //   // return new ActionAsPdf("Index");
        //}
        // GET: Technician/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technician technician = _db.Technicians.Find(id);
            if (technician == null)
            {
                return HttpNotFound();
            }
            return View(technician);
        }

        // GET: Technician/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Technician/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TechnicianId,TechnicianName,TechnicianPhone,TechnicianEmail,NumberOfOperations,Active")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                _db.Technicians.Add(technician);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(technician);
        }

        // GET: Technician/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technician technician = _db.Technicians.Find(id);
            if (technician == null)
            {
                return HttpNotFound();
            }
            return View(technician);
        }

        // POST: Technician/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TechnicianId,TechnicianName,TechnicianPhone,TechnicianEmail,NumberOfOperations,Active")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(technician).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(technician);
        }

        // GET: Technician/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technician technician = _db.Technicians.Find(id);
            if (technician == null)
            {
                return HttpNotFound();
            }
            return View(technician);
        }

        // POST: Technician/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Technician technician = _db.Technicians.Find(id);
            _db.Technicians.Remove(technician);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Operations(int id)
        {
            return View(_db.RepairCases.Where(s => s.TechnicianIdd == id));
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
