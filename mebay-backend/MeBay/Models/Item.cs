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

        public string Picture { get; set; } =
            "https://www.freeiconspng.com/uploads/no-image-icon-11.PNG";

        public int Price { get; set; } = 0;
        public int InstantPrice { get; set; } = 0;

        public DateTime ListingDate { get; set; } = DateTime.UtcNow;

        public DateTime SellingDate { get; set; } = DateTime.UtcNow.AddDays(7);

        [ForeignKey("User")]
        public required int UserId { get; set; }
        public required User User { get; set; }
    }
}
