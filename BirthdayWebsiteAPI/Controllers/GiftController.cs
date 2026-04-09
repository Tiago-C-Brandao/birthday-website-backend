using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.Models;
using BirthdayWebsiteAPI.ViewModels.Gift;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayWebsiteAPI.Controllers
{
    [Route("gift")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;

        public GiftController(IGiftService giftService)
        {
            _giftService = giftService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGift([FromBody] CreateGiftViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var gift = await _giftService.CreateGift(model);
                return Ok(gift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gift>>> GetAllGifts(string? giftName,
            string? productLink,
            bool? avaliable,
            string? userId)
        {
            try
            {
                var gifts = await _giftService.GetAllGifts(giftName, productLink, avaliable, userId);
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gift>> GetGiftById(int id)
        {
            try
            {
                var gift = await _giftService.GetGiftById(id);
                return Ok(gift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGift(int id, [FromBody] UpdateGiftViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedGift = await _giftService.UpdateGift(id, model);
                return Ok(updatedGift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("users/{userId}/give/{giftId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GiveGift(string userId, int giftId, [FromBody] GiveGiftViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                UpdateGiftViewModel updateModel = new UpdateGiftViewModel
                {
                    Available = true,
                    UserId = userId,
                    Message = model.Message,
                };

                var updatedGift = await _giftService.UpdateGift(giftId, updateModel);
                return Ok(updatedGift);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            try
            {
                await _giftService.DeleteGift(id);
                return Ok(new { message = "Gift deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
