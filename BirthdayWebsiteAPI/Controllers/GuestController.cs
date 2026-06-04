using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.Models.Enums;
using BirthdayWebsiteAPI.ViewModels.Guest;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayWebsiteAPI.Controllers
{
    [Route("api/guest")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGuest([FromBody] CreateGuestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var guest = await _guestService.CreateGuest(model);
            return Ok(guest);
        }

        [HttpPost("users/{userId}/accompanying")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddAccompanying(string userId,[FromBody] CreateGuestByUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var guest = await _guestService.CreateGuest(new CreateGuestViewModel
            {
                FullName = model.FullName,
                WhatsApp = model.WhatsApp,
                AccompanyingBy = userId,
                Status = GuestStatus.Pending,
            });
            return Ok(guest);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Guest>>> GetAllGuests(
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
            var guests = await _guestService.GetAllGuests(
                accompanyingBy, 
                userId, 
                fullName, 
                whatsapp, 
                hasUser, 
                hasAccompanying, 
                status, 
                pageNumber, 
                pageSize);
            return Ok(guests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(int id)
        {
            var guest = await _guestService.GetGuest(id);
            return Ok(guest);
        }

        [HttpGet("users/{userId}/accompanyings")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Guest>> GetAllAccompanying(string userId)
        {
            var guest = await _guestService.GetAllGuests(accompanyingBy: userId);
            return Ok(guest.Items);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGuest(int id, [FromBody] UpdateGuestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var guest = await _guestService.UpdateGuest(id, model);
            return Ok(guest);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            await _guestService.DeleteGuest(id);
            return Ok(new { message = "Guest deleted successfully" });
        }
    }
}
