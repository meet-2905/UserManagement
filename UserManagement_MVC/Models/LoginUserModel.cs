﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UserManagement_MVC.Models
{
    public class LoginUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}