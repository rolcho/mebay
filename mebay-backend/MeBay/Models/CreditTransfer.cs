using System.ComponentModel.DataAnnotations;

namespace MeBay.Models
{
    public class CreditTransfer
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required int Credit { get; set; }
    }
}
