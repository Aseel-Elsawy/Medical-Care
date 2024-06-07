using GraduationProject.Data.Models;
using GraduationProject.Migrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Data
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Favorite> favorites { get; set; }
        public DbSet<Appointment> appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Doctor)
                .WithMany(d => d.Favorites)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Patient)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AppUser>()
                   .HasOne(u => u.PatientProfile)
                   .WithMany()
                   .HasForeignKey(u => u.patient_id);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.DoctorProfile)
                .WithMany()
                .HasForeignKey(u => u.doctor_id);
        }
    }
}
