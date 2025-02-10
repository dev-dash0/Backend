using System.ComponentModel.DataAnnotations;

namespace DevDash.DTO
{
    public class LoginDto
    {

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; }

    }
}
