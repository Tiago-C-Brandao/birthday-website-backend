using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.Models.Enums;
using BirthdayWebsiteAPI.ViewModels;
using BirthdayWebsiteAPI.ViewModels.Guest;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IGuestService
    {
        Task<PagedResultViewModel<Guest>> GetAllGuests(
            string? accompanyingBy = null, 
            string? userId = null, 
            string? fullName = null, 
            string? whatsapp = null, 
            bool? hasUser = null, 
            bool? hasAccompanying = null,
            GuestStatus? status = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<Guest> GetGuest(int id);
        Task<Guest> CreateGuest(CreateGuestViewModel model);
        Task<Guest> UpdateGuest(int id, UpdateGuestViewModel model);
        Task<Guest> GetGuestByWhatsApp(string whasapp);
        Task DeleteGuest(int id);

    }
}
