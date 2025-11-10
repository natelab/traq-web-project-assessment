using System.ComponentModel.DataAnnotations;
using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")] //A username is a MUST
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")] //A password is a MUST
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
