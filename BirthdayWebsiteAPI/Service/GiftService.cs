using BirthdayWebsiteAPI.Data;
using BirthdayWebsiteAPI.Helpers;
using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels;
using BirthdayWebsiteAPI.ViewModels.Gift;
using Microsoft.EntityFrameworkCore;

namespace BirthdayWebsiteAPI.Service
{
    public class GiftService : IGiftService
    {
        private readonly AppDbContext _context;
        private readonly EntityUpdates _entityUpdates;

        public GiftService(AppDbContext context, EntityUpdates entityUpdates)
        {
            _context = context;
            _entityUpdates = entityUpdates; 
        }

        public async Task<Gift> CreateGift(CreateGiftViewModel model)
        {
            Gift gift = new Gift
            {
                GiftName = model.GiftName,
                ProductLink = model.ProductLink,
                ImageLink = model.ImageLink,
                Author = model.Author,
                Price = model.Price,
                Rarity = model.Rarity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();

            return gift;
        }

        public async Task<PagedResultViewModel<Gift>> GetAllGifts(
            string? giftName,
            string? productLink,
            bool? avaliable,
            string? userId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Gifts.AsQueryable();

            if (!string.IsNullOrEmpty(giftName))
                query = query.Where(g => g.GiftName.Contains(giftName));
            if (!string.IsNullOrEmpty(productLink))
                query = query.Where(g => g.ProductLink.Contains(productLink));
            if (avaliable.HasValue)
                query = query.Where(g => g.Available == avaliable);
            if (!string.IsNullOrEmpty(userId))
                query = query.Where(g => g.UserId == userId);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .OrderBy(g => g.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultViewModel<Gift>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Gift> GetGiftById(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);

            if (gift == null)
                throw new Exception("Gift not found");

            return gift;
        }

        public async Task<Gift> UpdateGift(int id, UpdateGiftViewModel model)
        {
            var currentGift = await _context.Gifts.FirstOrDefaultAsync(g => g.Id == id);

            if (currentGift == null)
                throw new Exception("Gift not found");

            _entityUpdates.UpdateProperties(currentGift, model);
            await _context.SaveChangesAsync();
            return currentGift;
        }

        public async Task DeleteGift(int id)
        {
            var gift = await _context.Gifts.FirstOrDefaultAsync(g => g.Id == id);

            if (gift == null)
                throw new Exception("Gift not found");

            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();
        }
    }
}
