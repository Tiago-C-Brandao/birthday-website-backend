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

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        private string FormatWhatsappNumber(string whatsappNumber)
        {
            // Remove all non-numeric characters using regex
            string cleanedPhone = Regex.Replace(whatsappNumber, @"[^\d]", "");

            // If the phone number starts with "55" and has 13 digits, it already includes the country code
            if (cleanedPhone.Length == 13 && cleanedPhone.StartsWith("55"))
            {
                return "+" + cleanedPhone; 
            }
            else if (cleanedPhone.Length == 11)
            {
                // If it's a valid 11-digit number (without +55), prepend "+55"
                return "+55" + cleanedPhone;
            }
            else
            {
                throw new ArgumentException("Invalid phone number format.");
            }
        }

        private bool IsPhoneNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var trimmed = input.Trim();

            // Remove all non-numeric characters using regex
            string cleanedPhone = Regex.Replace(trimmed, @"[^\d]", "");

            // Check if the cleaned phone number has the correct length (13 digits for +55XXXXXXXXXXX or 11 digits for XXXXXXXXXXX)
            if (cleanedPhone.Length == 11)
            {
                return true;  // Valid phone number with 11 digits (e.g., 81995084567)
            }
            else if (cleanedPhone.Length == 13 && cleanedPhone.StartsWith("55")) 
            {
                return true;  // Valid phone number with +55 (e.g., +55819995084567)
            }
            else
            {
                return false;
            }
        }


        public async Task<LoginInfo> Login(LoginViewModel model)
        {

            bool isPhone = IsPhoneNumber(model.UserNameOrWhatsapp);

            if (isPhone)
            {
                var formattedWhatsApp = FormatWhatsappNumber(model.UserNameOrWhatsapp);
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

        public async Task<bool> RegisterUser(RegisterViewModel model)
        {
            var userNameValidate = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            var whatsappValidate = await _userManager.Users.FirstOrDefaultAsync(u => u.WhatsApp == model.WhatsApp);

            if (userNameValidate != null)
            {
                throw new Exception("The username is already in use.");
            }

            if (whatsappValidate != null)
            {
                throw new Exception("The whatsapp is already in use.");
            }


            var formatedWhatsApp = FormatWhatsappNumber(model.WhatsApp);

            var newUser = new User
            {
                UserName = model.UserName,
                FullName = model.FullName,
                WhatsApp = formatedWhatsApp
            };

            var createdUser = await _userManager.CreateAsync(newUser, model.Password);
            var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
            return createdUser.Succeeded;
        }
    }
}
