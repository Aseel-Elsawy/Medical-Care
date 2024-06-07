namespace GraduationProject.Models
{
    public class PatientMdl
    {
        public string? Name { get; set; }

        public string? phone { get; set; }

        public string? Email { get; set; }
        public DateTime? Date_Of_birth { get; set; }

        public string ?Gender { get; set; }
        public IFormFile? Image { get; set; }
    }
}
