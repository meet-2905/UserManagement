using System.ComponentModel.DataAnnotations;

namespace UserManagement_MVC.Models
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage ="Username is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Firstname is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public string Role { get; set; }
       
        [EmailAddress]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength =6, ErrorMessage ="Password must be longer than 6 letters")]
        public string Password { get; set; }
    }
}
