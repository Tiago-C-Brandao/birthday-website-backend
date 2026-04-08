using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Guest;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayWebsiteAPI.Controllers
{
    [Route("guest")]
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
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var guest = await _guestService.CreateGuest(model);

                return Ok(guest);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/addAccompanying")]
        public async Task<IActionResult> AddAccompanying(string id,[FromBody] CreateGuestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var guest = await _guestService.CreateGuest(new CreateGuestViewModel
                {
                    FullName = model.FullName,
                    WhatsApp = model.WhatsApp,
                    AccompanyingBy = id
                });

                return Ok(guest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Guest>>> GetAllGuests(
            string? accompanyingBy = null,
            string? userId = null,
            string? fullName = null,
            string? whatsapp = null,
            bool? hasUser = null,
            bool? hasAccompanying = null)
        {
            try
            {
                var guests = await _guestService.GetAllGuests(accompanyingBy, userId, fullName, whatsapp, hasUser, hasAccompanying);
                return Ok(guests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(int id)
        {
            try
            {
                var guest = await _guestService.GetGuest(id);
                return Ok(guest);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/listAccompanyings")]
        public async Task<ActionResult<Guest>> GetAllAccompanying(string id)
        {
            try
            {
                var guest = await _guestService.GetAllGuests(accompanyingBy: id);
                return Ok(guest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGuest(int id, [FromBody] UpdateGuestViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var guest = await _guestService.UpdateGuest(id, model);
                return Ok(guest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            try
            {
                await _guestService.DeleteGuest(id);
                return Ok(new { message = "Guest deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
