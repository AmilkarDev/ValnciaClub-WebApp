using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VTechClubApp.Models
{
    public class ReceptionDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? ReceptionDate { get; set; }

        public int RepairCaseCount { get; set; }
    }


    public class RegistrationStatus
    {
        public bool status { get; set; }
    }
  
}