using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevDash.model
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }

        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }

        public DateTime? Birthday { get; set; }

        [Phone]
        [MaxLength(50)]
        [RegularExpression(@"^[0-9\+]{10,15}$")]
        public required string PhoneNumber { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime JoinedDate { get; set; } = DateTime.Now;

        public DateTime? LastActiveDate { get; set; }

        [MaxLength(255)]
        public string? Avatar { get; set; }

        public bool Verified { get; set; } = false;

        // Navigation Properties
        public ICollection<UserTenant>? UserTenants { get; set; }
        public ICollection<TeamMember>? TeamMembers { get; set; }
        public ICollection<IssueAssignedUser>? IssueAssignedUsers { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PersonalTask>? PersonalTasks { get; set; }
        public ICollection<PrivateNote>? PrivateNotes { get; set; }
        public ICollection<Team>? SupervisedTeams { get; set; }
        public ICollection<Project>? ManagedProjects { get; set; }

    }
}
