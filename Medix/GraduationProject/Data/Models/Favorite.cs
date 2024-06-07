using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Data.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }


        public virtual Doctor? Doctor { get; set; }

        public virtual Patient? Patient { get; set; }
    }
}
