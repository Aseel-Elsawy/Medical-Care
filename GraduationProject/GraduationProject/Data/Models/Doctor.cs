using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
    
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public DateTime? Date_Of_Birth { get; set; }
        public string? Gender { get; set; }
        public string? Speciality { get; set; }
        public string? bio { get; set; }
        public string? Address { get; set; }
        public decimal? Wage { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? imagefile { get; set; }
       
       
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
