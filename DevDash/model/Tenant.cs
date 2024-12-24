using System.ComponentModel.DataAnnotations;

namespace DevDash.model
{
    public class Tenant
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Owner { get; set; }
        [Required]
        [MaxLength(255)]
        public required string TenantCode { get; set; }
        [Required]
        [RegularExpression("Company|My-WorkSpace")]
        public required string Type { get; set; }
        [MaxLength(255)]
        public string? TenantUrl { get; set; }
        [MaxLength(255)]
        public string? Keywords { get; set; }
        public byte[]? Image { get; set; }

        // Navigation Properties
        public required User OwnerUser { get; set; }
        public ICollection<Project>? Projects { get; set; }
        public ICollection<UserTenant>? UserTenants { get; set; }
    }
}
