using System.ComponentModel.DataAnnotations;

namespace MeBay.Models
{
    public class UserRegisterDto
    {
        [Required]
        [MinLength(3)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public required string Password { get; set; }
    }
}
