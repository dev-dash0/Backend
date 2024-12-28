using DevDash.DTO;
using DevDash.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
         
        public AccountController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        [HttpPost("Register")] //Post api/Account/Register 
        public async Task<IActionResult> Register(RegisterDto UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    FirstName = UserFromRequest.FirstName,
                    LastName = UserFromRequest.LastName,
                    UserName = UserFromRequest.Username,
                    Email = UserFromRequest.Email,
                    PhoneNumber = UserFromRequest.PhoneNumber,
                    Birthday = UserFromRequest.Birthday,
                };
                IdentityResult result = await userManager.CreateAsync(user, UserFromRequest.Password);

                if (result.Succeeded)
                {
                    return Ok("Created");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return BadRequest();
        }

        [HttpPost("Login")] //Post api/Account/Register 
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
