using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pakland.Models
{
    public class PropertyDetails
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        public int? Size { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string? DateOfPurchase { get; set; }

        public byte[]? Image { get; set; }

        [StringLength(10)]
        public string? ImageType { get; set; }
        public bool IsSold { get; set; } = false;
        [ForeignKey("User")]
        public string? ApplicationUserId { get; set; }


    }
}
