using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models.Authentication
{
    public class SignUp
    {
        [Required(ErrorMessage = "UserName Is Required")]
        public string Username {  get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
        public bool IsPatient { get; set; }

        public bool IsDoctor { get; set; }
        public string ?phone { get; set; }


        public DateTime? Date_Of_birth { get; set; }

        public string? Gender { get; set; }
      //  public IFormFile? Image { get; set; }
   
        public string? Speciality { get; set; }
        public string? bio { get; set; }
        public string? Address { get; set; }
        public decimal? Wage { get; set; }
       
    }
}
