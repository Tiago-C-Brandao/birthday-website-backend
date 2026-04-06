using BirthdayWebsiteAPI.Models;

namespace BirthdayWebsiteAPI.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user, string role);
    }
}
