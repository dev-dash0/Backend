using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DevDash.model
{
    public class TeamMember
    {
        [Key]
        [Column(Order = 0)]
        public int TeamId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MemberId { get; set; }

        // Navigation Properties
        public User Member { get; set; }
        public Team Team { get; set; }
    }
}
