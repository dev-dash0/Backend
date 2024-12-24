using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DevDash.model
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [StringLength(255)]
        public string? Type { get; set; }


        public int? SupervisorId { get; set; }


        // Navigation Properties
        public User? Supervisor { get; set; }
        public ICollection<TeamMember>? TeamMembers { get; set; } 
    }
}
