using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VTechClubApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {


        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ContactViewModel
    {
        [Required(ErrorMessage="You must include your name") ]
        [StringLength(20, MinimumLength = 5)]
        public string name { get; set; }
        [Required(ErrorMessage="You must include your email address")]
        [EmailAddress(ErrorMessage="That's not the right form of an email address")]
        public string email { get; set; }
         [Required(ErrorMessage="You must mention the subject ")]
        public string subject { get; set; }
         [Required(ErrorMessage="You must include a message ")]
        public string  Message { get; set; }
    }

    public class RegisterViewModel
    {

        public string Name { get; set; }


          [Required]
        public string FirstName { get; set; }
          [Required]
        public string LastName { get; set; }

          [DataType(DataType.PhoneNumber, ErrorMessage = "Not a valid form of Phone Number")]
          [RegularExpression("^[0-9]{10}$", ErrorMessage="not valid phone number")]
          [StringLength(12, ErrorMessage = "Your Phone Number Length should be between 10 and 12")]
          [Required(ErrorMessage = "A Phone Number Is required")]
          public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "UserName")]
        [System.Web.Mvc.Remote("CheckForDuplication", "User",ErrorMessage="Username already existed"   )]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required ( ErrorMessage="A password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
