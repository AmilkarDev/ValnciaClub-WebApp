
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace VTechClubApp.Models
{
    public class Technician
    {

        public int TechnicianId { get; set; }
        //[DataType(DataType.Text)]
        //[StringLength(30, MinimumLength = 3)]
        //[Required(ErrorMessage = "A Technician Name is required")]
        public string Username { get; set; }

        //[DataType(DataType.Text)]
        //[StringLength(30, MinimumLength = 3)]
        //[Required(ErrorMessage = "A Technician Name is required")]
        [DisplayName("TechnicianeName")]
        public string FirstName { get; set; }

        //[DataType(DataType.Text)]
        //[StringLength(30,MinimumLength=3)]
        //[Required(ErrorMessage="A Technician Name is required")]
        public string LastName { get; set; }
        //[DataType(DataType.PhoneNumber)]
        
        //[StringLength(10)]
        public string TechnicianPhone { get; set; }

        //[DataType(DataType.EmailAddress)]
        //[StringLength(128)]
        //[Required()]
        public string TechnicianEmail { get; set; }
        public int NumberOfOperations { get; set; }
        public bool Active { get; set; }



        public virtual ICollection<RepairCase> RepairCases { get; set; }

        public virtual ICollection<ToolTorepair> ToolsToRepair { get; set; }

    }
}