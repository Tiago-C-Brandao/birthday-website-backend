using BirthdayWebsiteAPI.Data;
using BirthdayWebsiteAPI.Exceptions;
using BirthdayWebsiteAPI.Helpers;
using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.Models.Enums;
using BirthdayWebsiteAPI.ViewModels;
using BirthdayWebsiteAPI.ViewModels.Guest;
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
                throw new WhatsappAlreadyInUseException(model.WhatsApp);
            }

            Guest guest = new Guest
            {
                FullName = model.FullName,
                WhatsApp = formatedWhatsApp,
                UserId = model.UserId ?? null,
                AccompanyingBy = model.AccompanyingBy ?? null,
                Status = model.Status ?? GuestStatus.Confirmed,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            return guest;
        }

        public async Task<PagedResultViewModel<Guest>> GetAllGuests(
            string? accompanyingBy = null, 
            string? userId = null, 
            string? fullName = null, 
            string? whatsapp = null,
            bool? hasUser = null,
            bool? hasAccompanying = null,
            GuestStatus? status = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

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
            if(status.HasValue)
                query = query.Where(g => g.Status == status.Value);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .OrderBy(g => g.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultViewModel<Guest>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Guest> GetGuest(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
                throw new GuestNotFoundException();
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
                throw new GuestNotFoundException();

            if (!string.IsNullOrEmpty(model.WhatsApp))
            {
                var formatedWhatsApp = _phoneNumberFormatterAndValidator.FormatWhatsappNumber(model.WhatsApp);

                model.WhatsApp = formatedWhatsApp;

                var whatsappValidate = await _context.Guests.FirstOrDefaultAsync(u => u.WhatsApp == model.WhatsApp);

                if (whatsappValidate != null)
                {
                    throw new WhatsappAlreadyInUseException(model.WhatsApp);
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
                throw new GuestNotFoundException();

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();

        }
    }
}
