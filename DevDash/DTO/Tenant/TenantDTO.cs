using DevDash.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevDash.DTO.User;


namespace DevDash.DTO.Tenant
{
    public class TenantDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public  string Name { get; set; }

        public string? Description { get; set; }

        [MaxLength(255)]
        public string? TenantUrl { get; set; }

        public string? Keywords { get; set; }
        public byte[]? Image { get; set; }

        [Required]
        public int OwnerID { get; set; }

        public UserDTO Owner { get; set; }



    }
}
