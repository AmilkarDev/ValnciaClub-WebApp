
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VTechClubApp.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [DisplayName("CustomerName")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //[DataType(DataType.Text  , ErrorMessage="Your User Name Type should be a text")]
        //[StringLength(30,MinimumLength=6 ,ErrorMessage="Your username Length should be between 6 and 30"  )]
        //[Required(ErrorMessage="A UserName Is Required")]
        //public string UserName { get; set; }
        [DataType(DataType.PhoneNumber , ErrorMessage="Not a valid form of Phone Number")]
        [RegularExpression("^[0-9]{10}$")]
        [StringLength(12 ,ErrorMessage="Your Phone Number Length should be between 10 and 12")]
        [Required(ErrorMessage="A Phone Number Is required")]
        public string CustomerPhone { get; set; }


        [DataType(DataType.EmailAddress , ErrorMessage="Invalid Email Format"  )]
        [StringLength(128, ErrorMessage="Email address Too long")]
        [Required(ErrorMessage="An Email Is Required")]
        [System.Web.Mvc.Remote("CheckForDuplication", "RegisterStudent", ErrorMessage = "Customer Email already existed")]
        public string CustomerEmail { get; set; }


        public int NumberOfRequests { get; set; }

        public virtual ICollection<RepairCase> RepairCases { get; set; }

        public virtual ICollection<CallBack> CallBacks { get; set; }
    }

    
}