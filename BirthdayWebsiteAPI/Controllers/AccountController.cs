using BirthdayWebsiteAPI.Interface;
using BirthdayWebsiteAPI.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayWebsiteAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRegistrationService _registrationService;


        public AccountController(IAccountService accountService, IRegistrationService registrationService)
        {
            _accountService = accountService;
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _registrationService.UserRegistration(model);
            return Created("User created successfully", user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var loginInfo = await _accountService.Login(model);
                return Ok(loginInfo);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
                await _accountService.Logout();
                return Ok();
        }
    }
}
