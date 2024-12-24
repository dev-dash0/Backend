using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevDash.model
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; } // Fix applied here
        [MaxLength(500)]
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        [RegularExpression("To do|Active|Completed|Pending|Canceled")]
        public string Status { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(User.Id))]
        public int ProjectManagerId { get; set; }

        [Required]
        public int TenantId { get; set; }
        [Required]
        [MaxLength(20)]
        [RegularExpression("Low|Medium|High|Urgent")]
        public string Priority { get; set; } = string.Empty;

        [Required]
        public Tenant Tenant { get; set; } = null!;
        [Required]
        public User Manager { get; set; } = null!;
        public ICollection<Sprint>? Sprints { get; set; }
        public ICollection<Team>? Teams { get; set; }
    }
}
