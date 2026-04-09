using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Gift;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> GetAllGifts(
            string? giftName, 
            string? productLink, 
            bool? avaliable, 
            string? userId);
        Task<Gift> GetGiftById(int id);
        Task<Gift> CreateGift(CreateGiftViewModel model);
        Task<Gift> UpdateGift(int id, UpdateGiftViewModel model);
        Task DeleteGift(int id);
    }
}
