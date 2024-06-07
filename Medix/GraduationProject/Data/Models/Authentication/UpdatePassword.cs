using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models.Authentication
{
    public class UpdatePassword
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;
       
        public string NewPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
