using GraduationProject.Data;
using GraduationProject.Data.Models;
using GraduationProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace GraduationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FavoritesController(AppDbContext db)
        {
            _db = db;
        }
        string DoctorUrl = "http://154.38.186.138:5000/images/";
       
        #region 
        [HttpGet("{patient_id}")]
        public async Task<IActionResult> GetAllFavourites(int patient_id)
        {

            var favorite = await _db.favorites.Include(f => f.Doctor).Where(x => x.PatientId == patient_id).Select(f => new
            {

                id = f.Doctor.Id,
                name = f.Doctor.Name,
                phone = f.Doctor.Phone,
                email = f.Doctor.Email,
                speciality = f.Doctor.Speciality,
                bio = f.Doctor.bio,
                address = f.Doctor.Address,
                wage = f.Doctor.Wage,
                Image =DoctorUrl+ f.Doctor.Image,

            }).ToListAsync();
            if (favorite == null || favorite.Count == 0)
            {
                return NotFound();
            }
            return Ok(favorite);
        }
        #endregion
        #region add favorite
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromForm] FavoriteMdl mdl)
        {
            var favorites = new Favorite
            {
                DoctorId = mdl.DoctorId,
                PatientId = mdl.PatientId,
            };
            await _db.favorites.AddAsync(favorites);
            await _db.SaveChangesAsync();
            return Ok(favorites);
        }
        #endregion
        #region deleted
        [HttpDelete("{favoriteId}")]
        public async Task<IActionResult> DeleteFavorite(int favoriteId)
        {
            var favorite = await _db.favorites.FindAsync(favoriteId);

            if (favorite == null)
            {
                return NotFound();
            }
            _db.favorites.Remove(favorite);
            _db.SaveChanges();
            return Ok(favorite);
        }
        #endregion



    }
}
