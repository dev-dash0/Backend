using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DevDash.model
{
    public class UserTenant
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int TenantId { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("^(Admin|Developer|Manager|Supervisor)$", ErrorMessage = "Invalid role")]
        public required string Role { get; set; }

        [Required]
        public bool AcceptedInvitation { get; set; } = false;

        [Required]
        public bool Invited { get; set; } = false;

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public required User User { get; set; }
        public required Tenant Tenant { get; set; }
    }

}
