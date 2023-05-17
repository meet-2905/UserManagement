using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace UserManagement_MVC.Models
{
    public class LoginUserModel
    {
        [Required(ErrorMessage ="User Name is Required")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Password must be longer than 6 letters")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
