using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.ViewModels.Gift
{
    public class UpdateGiftViewModel
    {
        [MaxLength(100, ErrorMessage = "The gift name needs 50 characters")]
        public string? GiftName { get; set; }
        [DataType(DataType.Url)]
        public string? ProductLink { get; set; }
        public bool? Available { get; set; } = false;
        public string? UserId { get; set; }
        public string? Message { get; set; }
    }
}
