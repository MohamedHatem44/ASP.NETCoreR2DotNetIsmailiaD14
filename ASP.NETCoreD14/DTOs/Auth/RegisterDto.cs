using System.ComponentModel.DataAnnotations;

namespace ASP.NETCoreD14.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
