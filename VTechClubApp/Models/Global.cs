using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VTechClubApp.Models
{
    [Table("Globals")]
    public class Global
    {

        public int id { get; set; }
        public bool RegistrationStatus { get; set; }
    }
}