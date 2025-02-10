using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DevDash.model
{
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : IdentityUser<int>
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public override string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public override string Email { get; set; } = string.Empty;

        public DateOnly? Birthday { get; set; }

        [Phone]
        [MaxLength(50)]
        [Required]
        //[RegularExpression(@"^[0-9\+]{10,15}$")]
        //[RegularExpression(@"^(010 | 011 | 012 | 015)\d{8}$")]
        public override string PhoneNumber { get; set; } = string.Empty;


        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastActiveDate { get; set; }

        public byte[]? Avatar { get; set; }

        public bool Verified { get; set; } = false;

        public String? Personality { get; set; }
      
        
    
        // Navigation Properties

        public ICollection<PersonalTask>? PersonalTasks { get; set; }
        public ICollection<PrivateNote>? PrivateNotes { get; set; }
        public ICollection<Tenant>? OwnedTenants { get; set; }
        public ICollection<Tenant>? JoinedTenants { get; set; }
        public ICollection<UserTenant>? UserTenants { get; set; }
        public ICollection<Project>? ManagedProjects { get; set; }

        public ICollection<Project>? WorkingProjects { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; }

        public ICollection<Sprint>? SprintsCreated { get; set; }

        public ICollection<Issue>? IssuesCreated { get; set; }

        public ICollection<Issue>? IssuesAssigned { get; set; }

        public ICollection<IssueAssignedUser>? IssueAssignedUsers { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
