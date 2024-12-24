using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevDash.model
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IssueId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public int CreatedBy { get; set; }

        // Navigation Properties
        [Required]
        [ForeignKey("IssueId")]
        public required Issue Issue { get; set; }
        [ForeignKey("CreatedBy")]
        public required User CreatedByUser { get; set; } 
    }
}
