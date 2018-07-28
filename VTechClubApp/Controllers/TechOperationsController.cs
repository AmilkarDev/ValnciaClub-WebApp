using System.Linq;
using System.Web;
using System.Web.Mvc;
using VTechClubApp.DAL;
using VTechClubApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;
namespace VTechClubApp.Controllers
{
    public class TechOperationsController : Controller
    {
       private readonly VTechContext vtc;
       private readonly ApplicationDbContext vtc1;
        public TechOperationsController()
        {
            vtc1 = new ApplicationDbContext();
            vtc = new VTechContext();
          //  cc = new List<Customer>();
        }
        public TechOperationsController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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



        public async  Task<ActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            Technician tc = vtc.Technicians.Where(x => x.TechnicianEmail == user.Email).FirstOrDefault();
            var rcs = vtc.RepairCases.Where(x => x.Situation == advancement.Registered || x.TechnicianIdd==tc.TechnicianId).ToList() ;

            return View(rcs);
        }

        public async Task<ActionResult> MyRepairCases()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            Technician tc = vtc.Technicians.Where(x => x.TechnicianEmail == user.Email).FirstOrDefault();
            var rcs = vtc.RepairCases.Where(x =>x.TechnicianIdd == tc.TechnicianId).ToList();

            return View(rcs);
        }

        public ActionResult registered()
        {
            var rc = vtc.RepairCases.Where(x => x.Situation == advancement.Registered).ToList();
            return View(rc);
        }

        public async Task<ActionResult> InProgress()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            Technician tc = vtc.Technicians.Where(x => x.TechnicianEmail == user.Email).FirstOrDefault();
            var rcs = vtc.RepairCases.Where(x => x.TechnicianIdd == tc.TechnicianId && x.Situation == advancement.InProgress ).ToList();

            return View(rcs);
        }


        public async Task<ActionResult> Finished()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            Technician tc = vtc.Technicians.Where(x => x.TechnicianEmail == user.Email).FirstOrDefault();
            var rcs = vtc.RepairCases.Where(x => x.TechnicianIdd == tc.TechnicianId && x.Situation == advancement.Finished).ToList();

            return View(rcs);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairCase repairCase = vtc.RepairCases.Find(id);
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
       
        public ActionResult Edit([Bind(Include = "RepairCaseId,Note,TechNote,ReceptionDate,CompletionDate,PickUpDate,LatestPickUpDate,Agreement,TechnicianIdd,CustomerIdd")] RepairCase repairCase)
        {
            if (ModelState.IsValid)
            {
                if ((repairCase.TechnicianIdd != null) && (repairCase.Situation == advancement.Registered)) repairCase.Situation = advancement.InProgress;
                if (repairCase.CompletionDate != null) repairCase.Situation = advancement.Finished;
                repairCase.Agreement = "Agree";
                vtc.Entry(repairCase).State = EntityState.Modified;
                vtc.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(repairCase);
        }
        public ActionResult Tools(int? id)
        {
            return View(vtc.ToolsToRepair.Where(t => t.RepairCaseIdd == id).ToList());
        }
        // GET: RepairCases/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairCase repairCase = vtc.RepairCases.Find(id);
            if (repairCase == null)
            {
                return HttpNotFound();

            }
            return View(repairCase);
        }
    }
}