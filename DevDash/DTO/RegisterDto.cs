using System.ComponentModel.DataAnnotations;

namespace DevDash.DTO
{
    public class RegisterDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public required string LastName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public  string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public  string Email { get; set; } = string.Empty;

        public DateOnly? Birthday { get; set; }

        [Phone]
        [MaxLength(50)]
        //[RegularExpression(@"^[0-9\+]{10,15}$")]
        [Required(ErrorMessage = "Phone number is required.")]
        public  string PhoneNumber { get; set; } = string.Empty;
    }
}
