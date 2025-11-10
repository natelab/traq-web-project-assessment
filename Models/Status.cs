using System.ComponentModel.DataAnnotations;

namespace traq_web_project_assessment.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; } // It has two states either "Open" or "Closed"

        
		
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>(); //One status has many accounts (a navigation property)
    }
}