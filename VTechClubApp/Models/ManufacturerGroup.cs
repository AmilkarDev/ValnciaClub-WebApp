using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace VTechClubApp.Models
{

   
    public class ManufacturerGroup
    {

        public string Manufacturer { get; set; }
        public int ManufacturerCount { get; set; }
    }
}