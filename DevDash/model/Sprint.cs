using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevDash.model
{
    public class Sprint
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }
        public string? Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [MaxLength(255)]
        [RegularExpression("Planned|In Progress|Completed")]
        public required string Status { get; set; }
        [Required]
        public required int CreatedBy { get; set; }
        public string? Summary { get; set; }

        [Required]
        public required int ProjectId { get; set; }

        // Navigation Properties

        public required User CreatedByUser { get; set; }
        public required Project Project { get; set; }
    }

}
