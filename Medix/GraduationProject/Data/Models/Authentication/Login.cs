using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models.Authentication
{
    public class Login
    {
        [Required(ErrorMessage ="Email Is Required")]
         public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
       
    }
}
