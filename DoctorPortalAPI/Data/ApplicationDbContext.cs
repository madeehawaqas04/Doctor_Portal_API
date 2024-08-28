using DoctorPortalAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorPortalAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

      //  public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var doctor = new IdentityRole("doctor");
            doctor.NormalizedName = "doctor";

            var assistant = new IdentityRole("assistant");
            assistant.NormalizedName = "assistant";

            builder.Entity<IdentityRole>().HasData(admin, doctor, assistant);


        }
    }
}
