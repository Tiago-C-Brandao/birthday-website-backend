using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Account;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IRegistrationService
    {
        Task<User> UserRegistration(RegisterViewModel model);
    }
}
