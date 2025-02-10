using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevDash.model
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("IssueId")]
        public int IssueId { get; set; }

        [Required]
        [StringLength(255)]
        public required string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; }

        [ForeignKey("CreatedBy")]
        public int? CreatedById { get; set; }

        public string? Name { get; set; }

        // Navigation Properties
        [Required]
        public required Issue Issue { get; set; }

        public  User? CreatedBy { get; set; }
    }
}
