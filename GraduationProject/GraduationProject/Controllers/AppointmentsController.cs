using GraduationProject.Data;
using GraduationProject.Data.Models;
using GraduationProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AppointmentsController(AppDbContext db)
        {
            _db = db;

        }
        string DoctorUrl = "http://154.38.186.138:5000/images/";
        string PatientUrl = "http://154.38.186.138:5000/patientsimages/";
        [HttpGet("doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctor_id)
        {


            var appointment = await _db.appointments.Include(a => a.Patient)
                .Where(a => a.DoctorId == doctor_id).Select(f => new
                {
                    appointment_id = f.Id,
                    Date = f.Date,
                    Time = f.Time,
                    patient_name = f.Patient.Name,
                    patient_phone = f.Patient.phone,
                    patient_email = f.Patient.Email,
                    patient_image = PatientUrl+f.Patient.Image



                }).ToListAsync();
            if (appointment == null || appointment.Count == 0)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        [HttpGet("patient")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patient_id)
        {


            var appointment = await _db.appointments.Include(a => a.Doctor)
               .Where(a => a.PatientId == patient_id).Select(a => new
               {
                   appointment_id = a.Id,
                   Date = a.Date,
                   Time = a.Time,
                   doctor_id = a.Doctor.Id,
                   doctor_name = a.Doctor.Name,
                   doctor_phone = a.Doctor.Phone,
                   doctor_email = a.Doctor.Email,
                   doctor_speciality = a.Doctor.Speciality,
                   doctor_bio = a.Doctor.bio,
                   doctor_address = a.Doctor.Address,
                   doctor_wage = a.Doctor.Wage,
                    doctor_Image =DoctorUrl+ a.Doctor.Image



               }).ToListAsync();
            if (appointment == null || appointment.Count == 0)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        [HttpGet("patient/doctor")]
        public async Task<IActionResult> GetAppointments(int doctor_id, int patient_id)
        {


            var appointment = await _db.appointments.Include(a => a.Patient)
                .Include(a => a.Doctor).Where(a => a.PatientId == patient_id && a.DoctorId == doctor_id).Select(f => new
                {
                    appointment_id = f.Id,
                    Date = f.Date,
                    Time = f.Time,
                    doctor_id = f.Doctor.Id,
                    doctor_name = f.Doctor.Name,
                    doctor_phone = f.Doctor.Phone,
                    doctor_email = f.Doctor.Email,
                    doctor_speciality = f.Doctor.Speciality,
                    doctor_bio = f.Doctor.bio,
                    doctor_address = f.Doctor.Address,
                    doctor_wage = f.Doctor.Wage,
                    doctor_Image =DoctorUrl+ f.Doctor.Image,
                    patient_id = f.Patient.Id,
                    patient_name = f.Patient.Name,
                    patient_phone = f.Patient.phone,
                    patient_email = f.Patient.Email,
                    patient_image =PatientUrl+ f.Patient.Image,


                }).ToListAsync();
            if (appointment == null || appointment.Count == 0)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromForm] AppointmentMdl mdl)
        {
            var date = new DateOnly(mdl.Year, mdl.Month, mdl.Day);
            var time = new TimeOnly(mdl.Hour, mdl.Minute);
            var appointment = new Appointment
            {
                Date = date,
                Time = time,
                DoctorId = mdl.DoctorId,
                PatientId = mdl.PatientId,

            };

            await _db.appointments.AddAsync(appointment);
            await _db.SaveChangesAsync();
            return Ok(appointment);


        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAppointment(int appointment_id)
        {
            var appointment = await _db.appointments.FindAsync(appointment_id);
            if (appointment == null)
            {
                return NotFound();
            }
            _db.appointments.Remove(appointment);
            _db.SaveChanges();
            return Ok(appointment);
        }
    }
}
