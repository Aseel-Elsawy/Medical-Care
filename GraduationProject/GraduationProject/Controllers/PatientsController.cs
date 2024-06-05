using GraduationProject.Data;
using GraduationProject.Data.Models;
using GraduationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace GraduationProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _web;
        public PatientsController(AppDbContext db, IWebHostEnvironment web)
        {
            _db = db;
            _web = web;
        }
    
        string PatientUrl = "http://154.38.186.138:5000/patientsimages/";
        #region get all patients
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patient = await _db.patients.Select(f => new
            {

              id = f.Id,
               name = f.Name,
               phone = f.phone,
                email = f.Email,
                gender=f.Gender,
                date_Of_birth=f.Date_Of_birth,
                image = PatientUrl + f.Image,
            }).ToListAsync();
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
        #endregion
        #region get  patients by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllPatients(int id)
        {
            var patient = await _db.patients.Where(x => x.Id == id).Select(f => new
            {

                id = f.Id,
                name = f.Name,
                phone = f.phone,
                email = f.Email,
                gender = f.Gender,
                date_Of_birth = f.Date_Of_birth,
                image = PatientUrl + f.Image,
            }).FirstOrDefaultAsync();
            if (patient == null)
            {
                return NotFound($"Patient Id {id} Not Found");
            }
            return Ok(patient);
        }
        #endregion

        #region update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromForm] PatientMdl mdl)
        {
            var patients = await _db.patients.FindAsync(id);
            if (patients == null)
            {
                return NotFound($"Patient Id {id} Not found ");
            }
            byte[]? imageBytes = null;

            string filename = patients.Image;
            if (mdl.Image != null)
            {
                var contentpath = _web.ContentRootPath;
                string uploadFolder = Path.Combine(contentpath, "wwwroot", "patientsimages");
                filename = Guid.NewGuid().ToString() + "_" + mdl.Image.FileName;
                string fullpath = Path.Combine(uploadFolder, filename);
             
               if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                using (var stream = new FileStream(fullpath, FileMode.Create))
                {
                    await mdl.Image.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(patients.Image))
                {
                    string oldImagePath = Path.Combine(uploadFolder, patients.Image); ;
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
            patients.Name = mdl.Name;

            patients.phone = mdl.phone;
            patients.Date_Of_birth = mdl.Date_Of_birth;
            patients.Image = filename;

            patients.Gender = mdl.Gender;




            await _db.SaveChangesAsync();
            string hostingUrl = "http://154.38.186.138:5000/patientsimages/";
            string imageUrl = $"{hostingUrl}patientsimages/{filename}";

            var response = new
            {
                patients.Id,
                patients.Name,
                patients.phone,
                patients.Date_Of_birth,
                patients.Gender,
                ImageUrl = imageUrl
            };

            return Ok(response);
        }
        #endregion
    }
}
