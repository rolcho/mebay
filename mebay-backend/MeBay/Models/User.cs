using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeBay.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Email { get; set; }

        public string Role { get; set; } = "User";

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }
    }
}
