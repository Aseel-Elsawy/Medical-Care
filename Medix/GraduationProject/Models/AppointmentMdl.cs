namespace GraduationProject.Models
{
    public class AppointmentMdl
    {
       
        //public DateOnly Date { get; set; }
        //public TimeOnly Time { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
