
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VTechClubApp.Models;

namespace VTechClubApp.DAL
{

    //public class VTechContext : IdentityDbContext<ApplicationUser>
   public class VTechContext :DbContext
   
    {
        public VTechContext() : base("VTechContext") { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<RepairCase> RepairCases { get; set; }
        public DbSet<CallBack> CallBacks { get; set; }
        public DbSet<ToolTorepair> ToolsToRepair { get; set; }
        public DbSet<Global> Globals { get; set; }
       


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<ToolTorepair>()
                .HasRequired(x => x.Technician)
                .WithMany(w => w.ToolsToRepair)
                .HasForeignKey(x => x.TechnicianId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ToolTorepair>()
                .HasRequired(x => x.RepairCase)
                .WithMany(r => r.Tools)
                .HasForeignKey(y => y.RepairCaseIdd)
                .WillCascadeOnDelete(false);

          

            modelBuilder.Entity<RepairCase>()
                .HasRequired(j => j.Technician)
                .WithMany(f => f.RepairCases)
                .HasForeignKey(j => j.TechnicianIdd)
                .WillCascadeOnDelete(false);
            

           

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}