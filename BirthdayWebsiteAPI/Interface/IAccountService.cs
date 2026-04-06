using BirthdayWebsiteAPI.ViewModels;

namespace BirthdayWebsiteAPI.Interface
{
    public interface IAccountService
    {
        Task<bool> RegisterUser(RegisterViewModel model);
        Task<LoginInfo> Login(LoginViewModel model);
        Task Logout();
    }
}
