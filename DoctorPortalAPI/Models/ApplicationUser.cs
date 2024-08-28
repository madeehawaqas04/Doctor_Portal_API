using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DoctorPortalAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
       // public string Name { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }
        
        // [Display(Name = "Phone No")]
        //public string? PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[]? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
