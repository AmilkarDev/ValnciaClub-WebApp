using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity;
using VTechClubApp.Models;
using System;
using System.Collections.Generic;
using VTechClubApp.DAL;




namespace VTechClubApp.Controllers
{
   // [AccessDeniedAuthorize(Roles = "Master")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext vtc;
        private readonly VTechContext vtc1;
        List<Customer> cc=new List<Customer>() ;
        public UserController()
        {
            vtc = new ApplicationDbContext();
            vtc1 = new VTechContext();
          //  cc = new List<Customer>();
        }

        public UserController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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


        // GET: Users
     //   [Authorize(Roles="Master")]
        public async Task<ActionResult> Index( string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
               // int k = int.Parse(param);
                return View(await UserManager.Users.Where(x => x.UserName == param).ToListAsync());
            }
            return View(await UserManager.Users.ToListAsync());

        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }
        
        // GET: Users/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.EmailValidation = "Please make sure ure registering via a valid email address , because you won't be able to login unless it's confirmed";
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel , params string[] selectedRoles )
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName= userViewModel.UserName,
                    FirstName=userViewModel.FirstName,
                    LastName=userViewModel.LastName,
                    Email=userViewModel.Email,
                };
                var result = await UserManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var resultt = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (! resultt.Succeeded  )
                        {
                            ModelState.AddModelError("", resultt.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First());
                    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                    return View();
                }
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        // GET: Users/Edit/5
        public  async  Task<ActionResult> Edit(int id)
        {

           ViewBag.ValidationWarning = "If you will edit the email address,  you will receive an email confirmation to the new email address" ;
           ViewBag.EmailValidation= "If you edit the email address make sure you replace with a valid one , because you woon't be able to login unless the new one is confirmed";
            //if(id==null)
            //{
            //    return  new  HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var user = await UserManager.FindByIdAsync(id);
            if(user==null)
            {
                return HttpNotFound();
            }
            var userRoles = await UserManager.GetRolesAsync(user.Id);
            return View(new EditUserViewModel()
            {
                Id=user.Id,
               UserName=user.UserName,
               FirstName=user.FirstName,
               LastName=user.LastName,
               Email=user.Email,
              
               RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
               {
                   Selected=userRoles.Contains(x.Name),
                   Text=x.Name,
                   Value=x.Name
               })
            } );

        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,UserName,FirstName,LastName")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                if (user.Email != editUser.Email)
                {
                    user.EmailConfirmed = false;
                    Customer c = vtc1.Customers.Where(x => x.CustomerEmail == user.Email).FirstOrDefault()  ;
                    c.CustomerEmail = editUser.Email;
                    vtc.SaveChanges();
                }

                user.UserName = editUser.UserName;
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.Email = editUser.Email;
                var userRoles = await UserManager.GetRolesAsync(user.Id);
                selectedRole = selectedRole ?? new string[] { };
                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }


                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
               // bool ro = UserManager.IsInRoleAsync(user.Id, "Technician");
                if (await UserManager.IsInRoleAsync(user.Id, "Technician"))
                {
                    List<Technician> tcc = new List<Technician>();                    
                    tcc = vtc1.Technicians.ToList();
                 //   if (tcc.Count > 0)
                    //{
                        Technician tt = tcc.Where(x => x.TechnicianEmail == user.Email).FirstOrDefault();
                        if (tt == null)
                        {
                            Technician tc = new Technician
                            {
                                TechnicianEmail = user.Email,
                                Username = user.UserName,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                TechnicianPhone = user.PhoneNumber,
                                Active = true,
                                NumberOfOperations = 0,

                            };
                            vtc1.Technicians.Add(tc);
                            vtc1.SaveChanges();
                        }
                    //}
                   
                }
                //else
                //{
                //    List<Technician> tcc = new List<Technician>();
                //    tcc = vtc1.Technicians.ToList();
                //    Technician tt = tcc.Where(x => x.TechnicianEmail == user.Email).FirstOrDefault();
                //    if (tt != null)
                //    {
                        


                //        vtc1.Technicians.Remove(tt);

                //    }
                //}



                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        // GET: Users/Delete/5
        public  async Task<ActionResult> Delete(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var user = await  UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                //if (id == null)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Master()
        {
            List<ApplicationUser> xx = new List<ApplicationUser>() ;


            var users =  UserManager.Users.ToList();
            foreach (var item in users ){
                var userRoles = await UserManager.GetRolesAsync(item.Id);
                if (userRoles.Contains("Master")) xx.Add(item);
               
            }
            return View(xx);
        }
        public async Task<ActionResult> Technician()
        {
            List<ApplicationUser> xx = new List<ApplicationUser>();


            var users = UserManager.Users.ToList();
            foreach (var item in users)
            {
                var userRoles = await UserManager.GetRolesAsync(item.Id);
                if (userRoles.Contains("Technician")) xx.Add(item);

            }
            return View(xx);
        }

        public async Task<ActionResult> Users()
        {
            List<ApplicationUser> xx = new List<ApplicationUser>();
            var users =  UserManager.Users.ToList();
            foreach (var item in users)
            {
                var userRoles = await UserManager.GetRolesAsync(item.Id);
                if (userRoles.Contains("User")) xx.Add(item);

            }
            return View(xx);
        }




        //Public registered people who never launched a repair Case

        public bool FindInCustomers(string email)
        {
            
            foreach (var item in vtc1.Customers)
            {

                if (item.CustomerEmail == email) {  return false; }
            }
           // Session["OurCustomers"] = cc;
            return true;
        }
        // People registred and they have  NO repair Case (finished, in progress, registered)
        public ActionResult Registered()
        {
            List<ApplicationUser> xx = new List<ApplicationUser>();
            var users = UserManager.Users.ToList();
            foreach (var item in users)
            {
                if(FindInCustomers(item.Email)) xx.Add(item);
            }
            return View(xx);
        }

        // People registred and they have a repair Case (finished, in progress, registered)
        public ActionResult Customers()
        {
            List<ApplicationUser> xx = new List<ApplicationUser>();
            var users = UserManager.Users.ToList();
            foreach (var item in users)
            {
                if (!FindInCustomers(item.Email)) xx.Add(item);
            }
            return View(xx);
        }

        //people with repairCases Finished
        public ActionResult PreviousCustomers()
        {
           // cc = Session["OurCustomers"] as List<Customer>;
            List<Customer> lc = new List<Customer>();
            List<int> lid = new List<int>();
            var rc = vtc1.RepairCases.Where(x=>x.Situation==advancement.Finished).ToList();
            foreach(var item in rc){
                if (!lid.Contains(item.CustomerIdd))
                lid.Add(item.CustomerIdd);
            }
            if(cc!=null)
            {
                foreach(var item in vtc1.Customers.ToList())
                {
                    if(lid.Contains(item.CustomerId) )
                    {
                        lc.Add(item);
                    }
                }
            }
            return View(lc);
        }

        //people with repairCase either registered or InProgress
        public ActionResult CurrentCustomers()
        {
           // cc = Session["OurCustomers"] as List<Customer>;
            List<Customer> lc = new List<Customer>();
            List<int> lid = new List<int>();
            var rc = vtc1.RepairCases.Where(x => (x.Situation == advancement.Registered||x.Situation==advancement.InProgress)).ToList();
            foreach (var item in rc)
            {
                if (!lid.Contains(item.CustomerIdd))
                lid.Add(item.CustomerIdd);
            }

                foreach (var item in vtc1.Customers.ToList())
                {
                    if (lid.Contains(item.CustomerId))
                    {
                        lc.Add(item);
                    }
                }

            return View(lc);
        }

        //Customers with registered repair Cases
        public ActionResult RegisteredCustomers()
        {
            cc = Session["OurCustomers"] as List<Customer>;
            List<Customer> lc = new List<Customer>();
            List<int> lid = new List<int>();
            var rc = vtc1.RepairCases.Where(x =>x.Situation == advancement.Registered).ToList();
            foreach (var item in rc)
            {
                if(!lid.Contains(item.CustomerIdd))
                lid.Add(item.CustomerIdd);
            }

                foreach (var item in vtc1.Customers.ToList())
                {
                    if (lid.Contains(item.CustomerId))
                    {
                        lc.Add(item);
                    }
                }
            
            return View(lc);
        }
        //Customers with InProgress Repair Cases
        public ActionResult InProgressCustomers()
        {
            cc = Session["OurCustomers"] as List<Customer>;
            List<Customer> lc = new List<Customer>();
            List<int> lid = new List<int>();
            var rc = vtc1.RepairCases.Where(x => x.Situation == advancement.InProgress).ToList();
            foreach (var item in rc)
            {
                if (!lid.Contains(item.CustomerIdd))
                lid.Add(item.CustomerIdd);
            }
       
            
                foreach (var item in vtc1.Customers.ToList())
                {
                    if (lid.Contains(item.CustomerId))
                    {
                        lc.Add(item);
                    }
                }
           
            return View(lc);
        }

        //public ActionResult
        
        public JsonResult CheckForDuplication(string username)
        {
            var data = UserManager.Users.Where(p => p.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (data != null)
            {
                return Json("Sorry, this name already exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }




    }
}
