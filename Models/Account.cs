using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace traq_web_project_assessment.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Account Name is required")]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal OutstandingBalance { get; set; } = 0;

        
        [Required]
        public int PersonId { get; set; } // Foreign Key

        [Required]
        public int StatusId { get; set; } = 1; // Default to "Open" (Id = 1)

        // Navigation properties
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

       
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); //One account can have many transactions
    }
}