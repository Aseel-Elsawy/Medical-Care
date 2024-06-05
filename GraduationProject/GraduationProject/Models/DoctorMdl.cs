using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Models
{
    public class DoctorMdl
    {
       
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public DateTime Date_Of_Birth { get; set; }
        public string? Gender { get; set; }
        public string? Speciality { get; set; }
        public string? bio { get; set; }
        public string? Address { get; set; }
        public decimal? Wage { get; set; }
        public string? imagepath {  get; set; }
       public IFormFile? Image { get; set; }
    }
}
