using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models.Authentication
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = null!;
        [Compare("Password",ErrorMessage ="The Password And Confirmation Password Don't Match")]
        public string ConfirmPassword { get; set;} = null!;
        public string Email { get; set; }= null!;   
        public string token { get; set; } = null!;  
   

    }

}
