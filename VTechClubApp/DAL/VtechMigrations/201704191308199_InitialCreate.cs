namespace VTechClubApp.DAL.VtechMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CallBack",
                c => new
                    {
                        CallBackId = c.Int(nullable: false, identity: true),
                        CallBackResult = c.String(),
                        Customer_CustomerId = c.Int(),
                        RepairCase_RepairCaseId = c.Int(),
                        Technician_TechnicianId = c.Int(),
                    })
                .PrimaryKey(t => t.CallBackId)
                .ForeignKey("dbo.Customer", t => t.Customer_CustomerId)
                .ForeignKey("dbo.RepairCase", t => t.RepairCase_RepairCaseId)
                .ForeignKey("dbo.Technician", t => t.Technician_TechnicianId)
                .Index(t => t.Customer_CustomerId)
                .Index(t => t.RepairCase_RepairCaseId)
                .Index(t => t.Technician_TechnicianId);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CustomerPhone = c.String(nullable: false, maxLength: 12),
                        CustomerEmail = c.String(nullable: false, maxLength: 128),
                        NumberOfRequests = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.RepairCase",
                c => new
                    {
                        RepairCaseId = c.Int(nullable: false, identity: true),
                        Note = c.String(maxLength: 255),
                        RegistrationDate = c.DateTime(),
                        ReceptionDate = c.DateTime(nullable: false),
                        CompletionDate = c.DateTime(),
                        PickUpDate = c.DateTime(),
                        LatestPickUpDate = c.DateTime(),
                        Agreement = c.String(),
                        TechnicianIdd = c.Int(nullable: false),
                        CustomerIdd = c.Int(nullable: false),
                        Situation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RepairCaseId)
                .ForeignKey("dbo.Customer", t => t.CustomerIdd, cascadeDelete: true)
                .ForeignKey("dbo.Technician", t => t.TechnicianIdd)
                .Index(t => t.TechnicianIdd)
                .Index(t => t.CustomerIdd);
            
            CreateTable(
                "dbo.Technician",
                c => new
                    {
                        TechnicianId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        TechnicianPhone = c.String(),
                        TechnicianEmail = c.String(),
                        NumberOfOperations = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TechnicianId);
            
            CreateTable(
                "dbo.ToolTorepair",
                c => new
                    {
                        ToolToRepairId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Manufacturer = c.String(),
                        Issue = c.String(),
                        Result = c.String(),
                        description = c.String(),
                        TechnicianId = c.Int(nullable: false),
                        RepairCaseIdd = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ToolToRepairId)
                .ForeignKey("dbo.RepairCase", t => t.RepairCaseIdd)
                .ForeignKey("dbo.Technician", t => t.TechnicianId)
                .Index(t => t.TechnicianId)
                .Index(t => t.RepairCaseIdd);
            
            CreateTable(
                "dbo.Diagnostic",
                c => new
                    {
                        DiagnosticId = c.Int(nullable: false, identity: true),
                        DiagnosticType = c.String(),
                        TechnicianId = c.Int(nullable: false),
                        ToolToRepairId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiagnosticId)
                .ForeignKey("dbo.ToolTorepair", t => t.ToolToRepairId, cascadeDelete: true)
                .Index(t => t.ToolToRepairId);
            
            CreateTable(
                "dbo.Globals",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RegistrationStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .Index(t => t.IdentityRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.CallBack", "Technician_TechnicianId", "dbo.Technician");
            DropForeignKey("dbo.RepairCase", "TechnicianIdd", "dbo.Technician");
            DropForeignKey("dbo.ToolTorepair", "TechnicianId", "dbo.Technician");
            DropForeignKey("dbo.ToolTorepair", "RepairCaseIdd", "dbo.RepairCase");
            DropForeignKey("dbo.Diagnostic", "ToolToRepairId", "dbo.ToolTorepair");
            DropForeignKey("dbo.RepairCase", "CustomerIdd", "dbo.Customer");
            DropForeignKey("dbo.CallBack", "RepairCase_RepairCaseId", "dbo.RepairCase");
            DropForeignKey("dbo.CallBack", "Customer_CustomerId", "dbo.Customer");
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.Diagnostic", new[] { "ToolToRepairId" });
            DropIndex("dbo.ToolTorepair", new[] { "RepairCaseIdd" });
            DropIndex("dbo.ToolTorepair", new[] { "TechnicianId" });
            DropIndex("dbo.RepairCase", new[] { "CustomerIdd" });
            DropIndex("dbo.RepairCase", new[] { "TechnicianIdd" });
            DropIndex("dbo.CallBack", new[] { "Technician_TechnicianId" });
            DropIndex("dbo.CallBack", new[] { "RepairCase_RepairCaseId" });
            DropIndex("dbo.CallBack", new[] { "Customer_CustomerId" });
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.Globals");
            DropTable("dbo.Diagnostic");
            DropTable("dbo.ToolTorepair");
            DropTable("dbo.Technician");
            DropTable("dbo.RepairCase");
            DropTable("dbo.Customer");
            DropTable("dbo.CallBack");
        }
    }
}
