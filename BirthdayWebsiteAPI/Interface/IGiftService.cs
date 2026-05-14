using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels;
using BirthdayWebsiteAPI.ViewModels.Gift;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IGiftService
    {
        Task<PagedResultViewModel<Gift>> GetAllGifts(
            string? giftName, 
            string? productLink, 
            bool? avaliable, 
            string? userId,
            int pageNumber = 1,
            int pageSize = 10);
        Task<Gift> GetGiftById(int id);
        Task<Gift> CreateGift(CreateGiftViewModel model);
        Task<Gift> UpdateGift(int id, UpdateGiftViewModel model);
        Task DeleteGift(int id);
    }
}
