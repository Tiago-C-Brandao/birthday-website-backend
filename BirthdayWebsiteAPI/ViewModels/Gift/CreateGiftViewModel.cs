using BirthdayWebsiteAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.ViewModels.Gift
{
    public class CreateGiftViewModel
    {
        [Required]
        [MaxLength(100, ErrorMessage = "The gift name needs 50 characters")]
        public string GiftName { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string ProductLink { get; set; }
        [DataType(DataType.Url)]
        public string? ImageLink { get; set; }
        public string? Author { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        [EnumDataType(typeof(GiftRarity))]
        public GiftRarity? Rarity { get; set; } = GiftRarity.Common;
    }
}
