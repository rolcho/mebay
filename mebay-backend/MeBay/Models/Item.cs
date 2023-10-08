using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeBay.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        public string? Picture { get; set; }

        public int Price { get; set; } = 0;
        public int InstantPrice { get; set; } = 0;

        public DateTime ListingDate { get; set; } = DateTime.UtcNow;

        public DateTime SellingDate { get; set; } = DateTime.UtcNow.AddDays(7);

        [ForeignKey("Seller")]
        public required int SellerId { get; set; }
        public required User Seller { get; set; }

        [ForeignKey("Buyer")]
        public int? BuyerId { get; set; }
        public User? Buyer { get; set; }
    }
}
