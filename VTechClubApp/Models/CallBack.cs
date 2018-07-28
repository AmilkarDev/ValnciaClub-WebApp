

namespace VTechClubApp.Models
{
    public class CallBack
    {
        public int CallBackId { get; set; }

        public string CallBackResult { get; set; }
       
        public virtual Technician Technician { get; set;} 
        public virtual Customer Customer { get; set; }
        public virtual RepairCase RepairCase { get; set; }
    }
}