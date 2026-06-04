using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Account;
using BirthdayWebsiteAPI.ViewModels.Guest;

namespace BirthdayWebsiteAPI.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IAccountService _accountService;
        private readonly IGuestService _guestService;

        public RegistrationService(IAccountService accountService, IGuestService guesService)
        {
            _accountService = accountService;
            _guestService = guesService;
        }

        public async Task<User> UserRegistration(RegisterViewModel model)
        {
            var user = await _accountService.RegisterUser(model);

            var existingGuest = await _guestService.GetGuestByWhatsApp(model.WhatsApp);

            if (existingGuest != null)
            {
                UpdateGuestViewModel guestUpdate = new UpdateGuestViewModel
                {
                    FullName = user.FullName,
                    UserId = user.Id,
                };
                await _guestService.UpdateGuest(existingGuest.Id, guestUpdate);
            }
            else
            {
                CreateGuestViewModel guest = new CreateGuestViewModel
                {
                    FullName = model.FullName,
                    WhatsApp = model.WhatsApp,
                    UserId = user.Id,
                };
                await _guestService.CreateGuest(guest);
            }

            return user;
        }
    }
}
