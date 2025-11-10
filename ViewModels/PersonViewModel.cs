using System.ComponentModel.DataAnnotations;

namespace traq_web_project_assessment.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ID Number is required")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number must be exactly 13 characters")]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [StringLength(200)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Phone]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        // Display properties
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {Surname}";

        public int AccountCount { get; set; }
        public bool CanDelete { get; set; }
    }
}