using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.ViewModels.Account;
using BirthdayWebsiteAPI.ViewModels.Guest;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayWebsiteAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IGuestService _guestService;


        public AccountController(IAccountService accountService, IGuestService guestService)
        {
            _accountService = accountService;
            _guestService = guestService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _accountService.RegisterUser(model); 

                var existingGuest = await _guestService.GetGuestByWhatsApp(model.WhatsApp);

                if (existingGuest != null) 
                {
                    UpdateGuestViewModel guestUpdate = new UpdateGuestViewModel
                    {
                        UserId = user.Id,
                    };
                    await _guestService.UpdateGuest(existingGuest.Id, guestUpdate);
                } else
                {
                    CreateGuestViewModel guest = new CreateGuestViewModel
                    {
                        FullName = model.FullName,
                        WhatsApp = model.WhatsApp,
                        UserId = user.Id,
                    };
                    await _guestService.CreateGuest(guest);
                }


                return Created("User created successfully", user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var loginInfo = await _accountService.Login(model);
                return Ok(loginInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountService.Logout();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
