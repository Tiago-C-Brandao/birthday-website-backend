using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Guest;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllGuests();
        Task<Guest> GetGuest(int id);
        Task<Guest> CreateGuest(CreateGuestViewModel model);
        Task<Guest> UpdateGuest(int id, UpdateGuestViewModel model);
        Task<Guest> GetGuestByWhatsApp(string whasapp);
        Task DeleteGuest(int id);

    }
}
