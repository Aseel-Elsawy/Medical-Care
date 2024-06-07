using GraduationProject.Data;
using GraduationProject.Data.Models;
using GraduationProject.Data.Models.Authentication;
using GraduationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagmentService.Models;
using UserManagmentService.Services;
using IEmailService = UserManagmentService.Services.IEmailService;



namespace GraduationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        public AuthenticationController(UserManager<AppUser> userManager, IEmailService emailService, IConfiguration configuration, SignInManager<AppUser> signInManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _signInManager = signInManager;
            _dbContext = dbContext;

        }
        #region CreateAccount
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] SignUp signup)
        {
            var userexsist = await _userManager.FindByEmailAsync(signup.Email);
            if (userexsist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { status = "Failed", message = "User Already Exists" });
            }
            AppUser user = new()
            {
                Email = signup.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = signup.Username,
                TwoFactorEnabled = true,

            };


            decimal? wage = signup.Wage != null ? (decimal)signup.Wage : null;
            var result = await _userManager.CreateAsync(user, signup.Password);
            if (result.Succeeded)
            {
                if (signup.IsPatient)
                {
                    var patientProfile = new Patient
                    {
                        Name = signup.Username,
                        phone = signup.phone,
                        Email = signup.Email,
                        Date_Of_birth = signup.Date_Of_birth,
                        Gender = signup.Gender,
                    };
                    _dbContext.patients.Add(patientProfile);
                    user.PatientProfile = patientProfile;
                }
                else if (signup.IsDoctor)
                {
                    var doctorProfile = new Doctor
                    {
                        Name = signup.Username,
                        Phone = signup.phone,
                        Email = signup.Email,
                        Date_Of_Birth = signup.Date_Of_birth,
                        Gender = signup.Gender,
                        Speciality = signup.Speciality,
                        bio = signup.bio,
                        Address = signup.Address,
                        Wage = wage,
                    };
                    _dbContext.doctors.Add(doctorProfile);
                    user.DoctorProfile = doctorProfile;
                }
                await _dbContext.SaveChangesAsync();
                return Ok("User created successfully.");

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { status = "Failed", message = "User Failed To Create" });
            }

        }
        #endregion

        #region login
        [HttpPost("login")]
        public async Task<IActionResult> login([FromForm] Login lgn)
        {
            var user = await _userManager.FindByEmailAsync(lgn.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, lgn.Password))
            {
                if (user.TwoFactorEnabled)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, lgn.Password, false, true);
                    var profile = GetUserProfile(user);
                    return Ok(new { profile });

                    return StatusCode(StatusCodes.Status200OK, new Response { status = "Success", message = "Welcome" });
                }
                else
                {
                    return Unauthorized();
                }



            }


            return Unauthorized();
        }
        #endregion

        #region resetpassword
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> forgtopassword([FromForm] string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {

                return StatusCode(StatusCodes.Status404NotFound, new Response { status = "Failed", message = "Couldn't Find User" });

            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string truncatedToken;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                truncatedToken = Convert.ToBase64String(tokenData);
            }
            var forgotpasswordlink = token;
            var message = new Message(new string[] { user.Email! }, "Medix OTP", forgotpasswordlink!);
            _emailService.SendEmail(message);
            return StatusCode(StatusCodes.Status200OK, new Response { status = "Success", message = $" We Have Sent OTP To {user.Email} Successfully" });
        }
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPassword rst)

        {
            var user = await _userManager.FindByEmailAsync(rst.Email);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { status = "Failed", message = "Couldn't Find User" });
            }


            var result = await _userManager.ResetPasswordAsync(user, rst.token, rst.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { status = "Failed", message = "Invalid OTP" });

            }

            return StatusCode(StatusCodes.Status200OK, new Response { status = "Success", message = "Password Changed" });
        }
        #endregion

        #region updatePassword
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> UpdatePassword([FromForm] UpdatePassword pass)
        {
            var user = await _userManager.FindByEmailAsync(pass.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, pass.CurrentPassword))
            {

                var password = await _userManager.ChangePasswordAsync(user, pass.CurrentPassword, pass.NewPassword);


            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { status = "Failed", message = "Couldn't Change Password" });
            }
            return StatusCode(StatusCodes.Status200OK, new Response { status = "Success", message = "Password Changed" });
        }
        #endregion
        #region getProfile
        private object GetUserProfile(AppUser user)
        {
            string DoctorUrl = "http://154.38.186.138:5000/images/";
            string PatientUrl= "http://154.38.186.138:5000/patientsimages/";

            if (user.patient_id.HasValue)
            {
                var patientProfile = _dbContext.patients.Where(p => p.Id == user.patient_id).Select(f => new
                {
                    ProfileType = "Patient",
                    patient_id = f.Id,
                    patient_name = f.Name,
                    patient_phone = f.phone,
                    patient_email = f.Email,
                    patient_date_of_birth = f.Date_Of_birth,
                    patien_gender = f.Gender,
                    patient_image = PatientUrl+f.Image,


                });
                if (patientProfile != null)
                {

                    return (patientProfile);

                }

            }
            else if (user.doctor_id.HasValue)
            {
                var doctorProfile = _dbContext.doctors.Where(d => d.Id == user.doctor_id).Select(f => new
                {
                    ProfileType = "Doctor",
                    doctor_id = f.Id,
                    doctor_name = f.Name,
                    doctor_phone = f.Phone,
                    doctor_email = f.Email,
                    doctor_speciality = f.Speciality,
                    doctor_bio = f.bio,
                    doctor_address = f.Address,
                    doctor_wage = f.Wage,
                    doctor_gender = f.Gender,
                    doctor_date_of_birth = f.Date_Of_Birth,
                    doctor_Image = DoctorUrl+f.Image

                });
                if (doctorProfile != null)
                {
                    return (doctorProfile);
                }
            }

            return new { ProfileType = "NoProfile", ProfileData = new object()
            };

            #endregion

        }
       
    }
}
