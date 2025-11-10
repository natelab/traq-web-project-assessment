using System.ComponentModel.DataAnnotations;

namespace traq_web_project_assessment.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ID Number is required")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number must be exactly 13 characters")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100)]
        public string Surname { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        // Navigation property - One Person has Many Accounts
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}