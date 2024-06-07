using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Data.Models
{
    public class AppUser: IdentityUser
    {
        public int? patient_id { get; set; }
        public int? doctor_id { get; set; }
        public virtual Doctor? DoctorProfile { get; set; }
        public virtual Patient? PatientProfile { get; set; }
    }
}
