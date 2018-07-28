

using System;
using System.Collections.Generic;
using VTechClubApp.Models;

namespace VTechClubApp.DAL
{
    public class VTechInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VTechContext>
    {
        protected override void Seed(VTechContext context)
        {

            var customers = new List<Customer>()
            {
                new Customer { CustomerId  = 10, CustomerEmail="mal@gmail.com" , FirstName = "hello" , CustomerPhone = "10254"} ,
                new Customer {  CustomerId  = 8,CustomerEmail="mal@gmail.com" , FirstName = "hello" , CustomerPhone = "10254"} ,
                new Customer { CustomerId  = 6, CustomerEmail="mal@gmail.com" , FirstName = "hello" , CustomerPhone = "10254"} ,
                new Customer {  CustomerId  = 15,CustomerEmail="mal@gmail.com" , FirstName = "hello" , CustomerPhone = "10254"} ,
            };

            customers.ForEach(s=> context.Customers.Add(s)  );
            context.SaveChanges();

            //var technicians = new List<Technician>()
            //{
            //    new Technician { TechnicianId = 1 ,TechnicianName="hello" ,TechnicianPhone ="45698" , TechnicianEmail= "hjkdshjghj"  } ,
            //    new Technician { TechnicianId = 2 ,TechnicianName="hello" ,TechnicianPhone ="45698 ", TechnicianEmail= "hjkdshjghj" } ,
            //    new Technician {  TechnicianId = 3 ,TechnicianName="hello" ,TechnicianPhone ="45698" , TechnicianEmail= "hjkdshjghj" } ,
            //    new Technician { TechnicianId = 4 , TechnicianName="hello" ,TechnicianPhone ="45698" , TechnicianEmail= "hjkdshjghj" } ,
            //    new Technician { TechnicianId = 5, TechnicianName="hello" ,TechnicianPhone ="45698" , TechnicianEmail= "hjkdshjghj" } ,

            //};
            //technicians.ForEach(t => context.Technicians.Add(t));
            //context.SaveChanges();



            var repairCases = new List<RepairCase>()
            {
             new RepairCase { RepairCaseId=1 ,  TechnicianIdd=1 ,CustomerIdd=1 , CompletionDate= new DateTime(2010/10/10) , PickUpDate= new DateTime(2010/10/10) , ReceptionDate= new DateTime(2010/10/10)   }  , 
            new RepairCase { RepairCaseId=2 ,  TechnicianIdd=2,CustomerIdd=2 , CompletionDate= new DateTime(2010/10/10) , PickUpDate= new DateTime(2010/10/10) , ReceptionDate= new DateTime(2010/10/10)   }  ,
             new RepairCase { RepairCaseId=3 ,  TechnicianIdd=3 ,CustomerIdd=3 , CompletionDate= new DateTime(2010/10/10) , PickUpDate= new DateTime(2010/10/10) , ReceptionDate= new DateTime(2010/10/10)   }  ,
            
            };

            repairCases.ForEach(r => context.RepairCases.Add(r));
            context.SaveChanges();



            var toolsToRepair = new List<ToolTorepair>()
            {
                new ToolTorepair {ToolToRepairId = 100, Manufacturer = "dell", Issue = "Monitor luminosity" , RepairCaseIdd=1 , TechnicianId=1  },
                new ToolTorepair {ToolToRepairId = 200, Manufacturer = "dell", Issue = "Monitor luminosity",RepairCaseIdd=2 , TechnicianId=2},
                new ToolTorepair {ToolToRepairId = 300, Manufacturer = "dell", Issue = "Monitor luminosity", RepairCaseIdd=2 , TechnicianId=2 },
                new ToolTorepair {ToolToRepairId = 400, Manufacturer = "dell", Issue = "Monitor luminosity",RepairCaseIdd=2 , TechnicianId=3  },
                new ToolTorepair {ToolToRepairId = 500, Manufacturer = "dell", Issue = "Monitor luminosity", RepairCaseIdd=3 , TechnicianId=4 },
                new ToolTorepair {ToolToRepairId = 600, Manufacturer = "dell", Issue = "Monitor luminosity",RepairCaseIdd=3 , TechnicianId=5 },
                new ToolTorepair {ToolToRepairId = 700, Manufacturer = "dell", Issue = "Monitor luminosity",RepairCaseIdd=3 , TechnicianId=5  },
            };

            toolsToRepair.ForEach(tor => context.ToolsToRepair.Add(tor)   );
            context.SaveChanges();
         


        }
    }
}