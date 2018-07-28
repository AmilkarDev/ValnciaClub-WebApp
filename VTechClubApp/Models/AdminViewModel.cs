﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
namespace VTechClubApp.Models
{

        public class RoleViewModel
        {
            public int Id { get; set; }
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "RoleName")]
            public string Name { get; set; }
            public string Description { get; set; }
        }



        public class EditUserViewModel
        {
            public int Id { get; set; }
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string UserName { get; set; }
            public IEnumerable<SelectListItem> RolesList { get; set; }
        }
     

}