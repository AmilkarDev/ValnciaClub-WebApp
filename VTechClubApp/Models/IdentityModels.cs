using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace VTechClubApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim> ,IUser<int>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser , int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }


    public class CustomRole : IdentityRole<int, CustomUserRole>,IRole<int>
    {
        //public CustomRole() { }
        //public CustomRole(string name) { Name = name; }
        //public string Description { get; set; }
         public string Description { get; set; }
  
    public CustomRole() { }
    public CustomRole(string name)
        : this()
    {
        this.Name = name;
    }

    public CustomRole(string name, string description)
        : this(name)
    {
        this.Description = description;
    }
    } 

    //public class ApplicationRole : IdentityRole
    //{
    //    public ApplicationRole() : base() { }
    //    public ApplicationRole(string name) : base(name) { }
    //    public string Description { get; set; }

    //}
    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
    CustomUserLogin, CustomUserRole, CustomUserClaim>, IUserStore<ApplicationUser, int>,
        IDisposable
    {
        //public CustomUserStore()
        //    : this(new IdentityDbContext())
        //{
        //    base.DisposeContext = true;
        //}
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>, IQueryableRoleStore<CustomRole, int>,
        IRoleStore<CustomRole, int>, IDisposable
    {
        //public CustomRoleStore()
        //    : base(new IdentityDbContext())
        //{
        //    base.DisposeContext = true;
        //}
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim> 
    {
        public ApplicationDbContext()
            : base("VTechContext")
            //: base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       // public System.Data.Entity.DbSet<VTechClubApp.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<VTechClubApp.Models.RoleViewModel> RoleViewModels { get; set; }

        //public System.Data.Entity.DbSet<VTechClubApp.Models.ApplicationRole> IdentityRoles { get; set; }

        //public System.Data.Entity.DbSet<VTechClubApp.Models.EditUserViewModel> EditUserViewModels { get; set; }




       // public System.Data.Entity.DbSet<VTechClubApp.Models.ApplicationUser> ApplicationUsers { get; set; }

      //  public System.Data.Entity.DbSet<VTechClubApp.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}