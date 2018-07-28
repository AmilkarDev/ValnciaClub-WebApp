using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VTechClubApp.Models
{



    public enum advancement
    {
        Registered,
        InProgress,
        Finished
    }


    public class RepairCase
    {
        public int RepairCaseId { get; set; }
      
       //[DataType(DataType.Text)]
        //[MaxLength ]
        [DisplayName("Customer Note")]
        [StringLength(255)]
        [DataType(DataType.MultilineText )]
        public string Note { get; set; }
       
       [DataType(DataType.Date)]
       [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
       public Nullable<DateTime> RegistrationDate { get; set; }


        [Display(Name="Reception Date")]
        [Required (ErrorMessage="A Reception Date is required ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public  DateTime ReceptionDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> CompletionDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> PickUpDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> LatestPickUpDate { get; set; }

        public string Agreement { get; set; }
        [DisplayName("TechnicianId")]
        public Nullable<int> TechnicianIdd { get; set; }
        [DisplayName("CustomerId")]
        public int CustomerIdd { get; set; }


        public advancement Situation { get; set; }

         [DataType(DataType.MultilineText)]
        [DisplayName("Technical Note")]
        public string TechNote { get; set; }


        [ForeignKey("CustomerIdd")]
        public virtual Customer Customer { get; set; }

         [ForeignKey("TechnicianIdd")]
        public  virtual Technician Technician { get; set; }
        public virtual ICollection<CallBack> CallBacks { get; set; }
        public virtual ICollection<ToolTorepair>  Tools { get; set; }
    
    }
}