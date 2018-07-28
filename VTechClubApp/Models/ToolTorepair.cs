using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VTechClubApp.Models
{
    public class ToolTorepair
    {
        public int  ToolToRepairId { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "A Tool Type is required  is required")]
        public string Type { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "A Tool manufacturer is required  is required")]
        public string  Manufacturer { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "A Tool Issue is required  is required")]
        public string  Issue { get; set; }

        public  string Result  { get; set; }

        public string description { get; set; }


       
        public Nullable<int> TechnicianId { get; set; }
     
        public int RepairCaseIdd { get; set; }
        

         [ForeignKey("TechnicianId")]
        public virtual Technician Technician { get; set; }
         [ForeignKey("RepairCaseIdd")]
        public virtual   RepairCase RepairCase { get; set; }
        public virtual ICollection<Diagnostic> Diagnostics { get; set; }

    }
}