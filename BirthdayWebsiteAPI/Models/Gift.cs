using System.ComponentModel.DataAnnotations;

namespace BirthdayWebsiteAPI.Models
{
    public class Gift
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The gift name needs 50 characters")]
        public string GiftName { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string ProductLink { get; set; }
        public bool Available { get; set; } = false;
        public string? UserId { get; set; } // UserId of the guest who reserved it 
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
