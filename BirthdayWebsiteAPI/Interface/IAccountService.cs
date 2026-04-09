using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Account;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IAccountService
    {
        Task<User> RegisterUser(RegisterViewModel model);
        Task<LoginInfo> Login(LoginViewModel model);
        Task Logout();
    }
}
