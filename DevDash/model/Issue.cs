using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevDash.model
{
    public class Issue
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? Labels { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime? StartDate { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public DateTime? LastUpdate { get; set; }
        [MaxLength(20)]
        [RegularExpression("Bug|Feature|Task|Epic", ErrorMessage = "Invalid issue type.")]
        [Required]
        public required string Type { get; set; }

        [MaxLength(20)]
        [Required]
        [RegularExpression("Low|Medium|High|Critical", ErrorMessage = "Invalid priority.")]
        public required string Priority { get; set; }

        [MaxLength(20)]
        [Required]
        [RegularExpression("BackLog|to do|In Progress|Reviewing|Completed|Canceled|Postponed")]
        public required string Status { get; set; }
        [Required]
        [ForeignKey(nameof(Sprint.Id))]
        public required int SprintId { get; set; } // Foreign key
        [Required]
        [ForeignKey(nameof(Project.Id))]
        public required int  ProjectId { get; set; } // Foreign key
        public Sprint? Sprint { get; set; } // Navigation property

        [Required]
        public required int CreatedById { get; set; } // Foreign key
        [Required]
        public required User CreatedBy { get; set; } // Navigation property

        public ICollection<User>? AssignedUsers { get; set; }

        public ICollection<IssueAssignedUser>? IssueAssignedUsers { get; set; }

    }
}
