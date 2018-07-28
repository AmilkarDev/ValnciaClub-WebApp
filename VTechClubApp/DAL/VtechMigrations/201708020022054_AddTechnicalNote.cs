namespace VTechClubApp.DAL.VtechMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTechnicalNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RepairCase", "TechNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RepairCase", "TechNote");
        }
    }
}
