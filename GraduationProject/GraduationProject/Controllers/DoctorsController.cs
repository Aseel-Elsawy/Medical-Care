using GraduationProject.Data;
using GraduationProject.Data.Models;
using GraduationProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace GraduationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _web;
        public DoctorsController(AppDbContext db, IWebHostEnvironment web)
        {
            _db = db;
            _web = web;
        }
        string DoctorUrl = "http://154.38.186.138:5000/images/";

        #region get all doctors
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {

            var doctor = await _db.doctors.Select(f => new
            {
               
               id = f.Id,
               name = f.Name,
               phone = f.Phone,
               email = f.Email,
               speciality = f.Speciality,
                bio = f.bio,
                date_Of_Birth=f.Date_Of_Birth,
               address = f.Address,
                wage = f.Wage,
                gender=f.Gender,
                image = DoctorUrl+f.Image
            }).ToListAsync();
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }
        #endregion
        #region get doctor by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllDoctors(int id)
        {
            var doctor = await _db.doctors.Where(x => x.Id == id).Select(f => new
            {

                id = f.Id,
                name = f.Name,
                phone = f.Phone,
                email = f.Email,
                speciality = f.Speciality,
                bio = f.bio,
                date_Of_Birth = f.Date_Of_Birth,
                address = f.Address,
                wage = f.Wage,
                gender = f.Gender,
                image = DoctorUrl + f.Image
            }).FirstOrDefaultAsync();
            if (doctor == null)
            {
                return NotFound($"Doctor Id {id} Not Found");
            }
            return Ok(doctor);
        }
        #endregion
        #region get doctor by specilization
        [HttpGet("GetDoctor/{specilization}")]
        public async Task<IActionResult> GetDoctorsBySpecilization(string specilization)
        {
            var doctor = await _db.doctors.Where(x => x.Speciality == specilization).Select(f => new
            {

                id = f.Id,
                name = f.Name,
                phone = f.Phone,
                email = f.Email,
                speciality = f.Speciality,
                bio = f.bio,
                date_Of_Birth = f.Date_Of_Birth,
                address = f.Address,
                wage = f.Wage,
                gender = f.Gender,
                image = DoctorUrl + f.Image
            }).ToListAsync();
            if (doctor == null)
            {
                return NotFound($"Doctor Specilization {specilization} Not Found");
            }
            return Ok(doctor);
        }
        #endregion
        #region get doctor by name
        [HttpGet("SearchDoctor/{Name}")]
        public async Task<IActionResult> GetDoctorsByName(string Name)
        {
            var doctor = await _db.doctors.Where(x => x.Name == Name).Select(f => new
            {

                id = f.Id,
                name = f.Name,
                phone = f.Phone,
                email = f.Email,
                speciality = f.Speciality,
                bio = f.bio,
                date_Of_Birth = f.Date_Of_Birth,
                address = f.Address,
                wage = f.Wage,
                gender = f.Gender,
                image = DoctorUrl + f.Image
            }).ToListAsync();
            if (doctor == null)
            {
                return NotFound($"Doctor  {Name} Not Found");
            }
            return Ok(doctor);
        }
        #endregion

        #region Update doctor
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromForm] DoctorMdl mdl)
        {
            var doctor = await _db.doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor Id {id} Not found ");
            }

            string filename = doctor.Image;
            if (mdl.Image != null)
            {
                var contentPath = _web.ContentRootPath;
                string uploadFolder = Path.Combine(contentPath, "wwwroot","images");
                filename = Guid.NewGuid().ToString() + "_" + mdl.Image.FileName;
                string fullPath = Path.Combine(uploadFolder, filename);

  if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

       

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await mdl.Image.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(doctor.Image))
                {
                    string oldImagePath = Path.Combine(uploadFolder, doctor.Image);
                    try
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    catch (IOException ex)
                    {
                        return StatusCode(500, $"Error deleting old image: {ex.Message}");
                    }
                }
            }

            decimal? wage = mdl.Wage != null ? (decimal)mdl.Wage : null;

            doctor.Name = mdl.Name;
            doctor.Phone = mdl.Phone;
            doctor.Date_Of_Birth = mdl.Date_Of_Birth;
            doctor.Speciality = mdl.Speciality;
            doctor.bio = mdl.bio;
            doctor.Address = mdl.Address;
            doctor.Wage = wage;
            doctor.Gender = mdl.Gender;
            doctor.Image = filename;

            await _db.SaveChangesAsync();
            string hostingUrl = "http://154.38.186.138:5000/";
            string imageUrl = $"{hostingUrl}images/{filename}";


         

            var response = new
            {
                doctor.Id,
                doctor.Name,
                doctor.Phone,
                doctor.Date_Of_Birth,
                doctor.Speciality,
                doctor.bio,
                doctor.Address,
                doctor.Wage,
                doctor.Gender,
                ImageUrl = imageUrl
            };

            return Ok(response);
        }
        #endregion

    }
}
