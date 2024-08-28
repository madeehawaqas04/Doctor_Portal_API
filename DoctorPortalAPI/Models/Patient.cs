using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorPortalAPI.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(30)]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30)]
        public string? LastName { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Document")]
        public byte[]? Document { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

    }
}
