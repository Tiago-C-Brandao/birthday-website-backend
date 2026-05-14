using BirthdayWebsiteAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.Models
{
    public class Gift
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The gift name needs 50 characters")]
        public string GiftName { get; set; }
        public string? Author { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string ProductLink { get; set; }
        [DataType(DataType.Url)]
        public string? ImageLink { get; set; }
        public bool Available { get; set; } = false;
        public string? UserId { get; set; } // UserId of the guest who reserved it 
        public string? Message { get; set; }
        [EnumDataType(typeof(GiftRarity))]
        public GiftRarity? Rarity { get; set; } = GiftRarity.Common;
        public DateTime CreatedAt { get; set; }
    }
}
