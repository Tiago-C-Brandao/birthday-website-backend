using BirthdayWebsiteAPI.Helpers;
using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BirthdayWebsiteAPI.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly PhoneNumberFormatterAndValidator _phoneNumberFormatterAndValidator;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, PhoneNumberFormatterAndValidator formatPhoneNumber)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _phoneNumberFormatterAndValidator = formatPhoneNumber; 
        }
        public async Task<LoginInfo> Login(LoginViewModel model)
        {

            bool isPhone = _phoneNumberFormatterAndValidator.IsPhoneNumber(model.UserNameOrWhatsapp);

            if (isPhone)
            {
                var formattedWhatsApp = _phoneNumberFormatterAndValidator.FormatWhatsappNumber(model.UserNameOrWhatsapp);
                model.UserNameOrWhatsapp = formattedWhatsApp;
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserNameOrWhatsapp || u.WhatsApp == model.UserNameOrWhatsapp);

            if (user == null)
            {
                throw new Exception("Username or WhatsApp not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Username or password incorrect.");
            }

            var token = _tokenService.GenerateToken(user, role);

            return new LoginInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                WhatsApp = user.WhatsApp,
                Role = role,
                Token = token
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> RegisterUser(RegisterViewModel model)
        {
            var formatedWhatsApp = _phoneNumberFormatterAndValidator.FormatWhatsappNumber(model.WhatsApp);

            var userNameValidate = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            var whatsappValidate = await _userManager.Users.FirstOrDefaultAsync(u => u.WhatsApp == formatedWhatsApp);

            if (userNameValidate != null)
            {
                throw new Exception("The username is already in use.");
            }

            if (whatsappValidate != null)
            {
                throw new Exception("The whatsapp is already in use.");
            }

            var newUser = new User
            {
                UserName = model.UserName,
                FullName = model.FullName,
                WhatsApp = formatedWhatsApp,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userManager.CreateAsync(newUser, model.Password);
            var roleResult = await _userManager.AddToRoleAsync(newUser, "User");

            var result = new User
            {
                Id = newUser.Id,
                UserName = newUser.UserName,
                FullName = newUser.FullName,
                WhatsApp = newUser.WhatsApp,
                CreatedAt = newUser.CreatedAt
            };

            return result;
        }
    }
}
