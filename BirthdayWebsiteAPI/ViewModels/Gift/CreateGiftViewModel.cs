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
    }
}
