using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTechClubApp.Models
{
    public class Diagnostic
    {
        // We can add a property Named RESULT to define the result of this diagnostic and whether it succeeded in fixing the problem or not
        public int DiagnosticId { get; set; }
        public string  DiagnosticType { get; set; }







        
        public virtual int  TechnicianId { get; set; } //Set as virtual roperty to allow Lazy Loading , so Ef load the entity only whe it's required to do 
        public virtual int  ToolToRepairId { get; set; }
    }
}
