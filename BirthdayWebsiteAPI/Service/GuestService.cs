using BirthdayWebsiteAPI.Data;
using BirthdayWebsiteAPI.Helpers;
using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Guest;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BirthdayWebsiteAPI.Service
{
    public class GuestService : IGuestService
    {
        private readonly AppDbContext _context;
        private readonly EntityUpdates _entityUpdates;
        private readonly PhoneNumberFormatterAndValidator _phoneNumberFormatterAndValidator;

        public GuestService(AppDbContext context, EntityUpdates entityUpdates, PhoneNumberFormatterAndValidator phoneNumberFormatterAndValidator)
        {
            _context = context;
            _entityUpdates = entityUpdates;
            _phoneNumberFormatterAndValidator = phoneNumberFormatterAndValidator;
        }

        public async Task<Guest> CreateGuest(CreateGuestViewModel model)
        {
            var formatedWhatsApp = _phoneNumberFormatterAndValidator.FormatWhatsappNumber(model.WhatsApp);
            var whatsappValidate = await _context.Guests.FirstOrDefaultAsync(u => u.WhatsApp == formatedWhatsApp);

            if (whatsappValidate != null)
            {
                throw new Exception("The whatsapp is already in use.");
            }

            Guest guest = new Guest
            {
                FullName = model.FullName,
                WhatsApp = formatedWhatsApp,
                UserId = model.UserId ?? null,
                AccompanyingBy = model.AccompanyingBy ?? null
            };

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            return guest;
        }

        public async Task<IEnumerable<Guest>> GetAllGuests(
            string? accompanyingBy = null, 
            string? userId = null, 
            string? fullName = null, 
            string? whatsapp = null,
            bool? hasUser = null,
            bool? hasAccompanying = null)
        {
            var query = _context.Guests.AsQueryable();

            if (!string.IsNullOrEmpty(accompanyingBy))
                query = query.Where(g => g.AccompanyingBy == accompanyingBy);
            if(!string.IsNullOrEmpty(userId))
                query = query.Where(g => g.UserId == userId);
            if (!string.IsNullOrEmpty(fullName))
                query = query.Where(g => g.FullName.Contains(fullName));
            if (!string.IsNullOrEmpty(whatsapp))
                query = query.Where(g => g.WhatsApp.Contains(whatsapp));
            if(hasUser.HasValue && hasUser.Value)
                query = query.Where(g => g.UserId != null);
            if (hasUser.HasValue && hasUser.Value == false)
                query = query.Where(g => g.UserId == null);
            if (hasAccompanying.HasValue && hasAccompanying.Value)
                query = query.Where(g => g.AccompanyingBy != null);
            if (hasAccompanying.HasValue && hasAccompanying.Value == false)
                query = query.Where(g => g.AccompanyingBy == null);

            var guests = await query.ToListAsync();
            return guests;
        }

        public async Task<Guest> GetGuest(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
                throw new Exception("Guest not found!");
            return guest;
        }

        public async Task<Guest> GetGuestByWhatsApp(string whatsapp)
        {
            var formattedWhatsApp = _phoneNumberFormatterAndValidator.FormatWhatsappNumber(whatsapp);
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.WhatsApp == formattedWhatsApp);
            return guest;
        }

        public async Task<Guest> UpdateGuest(int id, UpdateGuestViewModel model)
        {
            var currentGuest = await _context.Guests.FirstOrDefaultAsync(g => g.Id == id);

            if (currentGuest == null)
                throw new Exception("Guest not found!");

            if(!string.IsNullOrEmpty(model.WhatsApp))
            {
                var formatedWhatsApp = _phoneNumberFormatterAndValidator.FormatWhatsappNumber(model.WhatsApp);

                model.WhatsApp = formatedWhatsApp;

                var whatsappValidate = await _context.Guests.FirstOrDefaultAsync(u => u.WhatsApp == model.WhatsApp);

                if (whatsappValidate != null)
                {
                    throw new Exception("The whatsapp is already in use.");
                }
            }

            _entityUpdates.UpdateProperties(currentGuest, model);
            await _context.SaveChangesAsync();
            return currentGuest;
        }

        public async Task DeleteGuest(int id)
        {
            var guest = await _context.Guests.FirstOrDefaultAsync(g => g.Id == id);
            if (guest == null)
                throw new Exception("Guest not found!");

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();

        }
    }
}
