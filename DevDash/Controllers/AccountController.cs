﻿using DevDash.DTO;
using DevDash.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<User> _userManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }

        [HttpPost("Register")] //Post api/Account/Register 
        public async Task<IActionResult> Register([FromBody] RegisterDto UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    FirstName = UserFromRequest.FirstName,
                    LastName = UserFromRequest.LastName,
                    UserName = UserFromRequest.UserName,
                    Email = UserFromRequest.Email,
                    PhoneNumber = UserFromRequest.PhoneNumber,
                    Birthday = UserFromRequest.Birthday,
                    JoinedDate = DateTime.UtcNow
                };
                IdentityResult result = await userManager.CreateAsync(user, UserFromRequest.Password);

                if (result.Succeeded)
                {
                    return Ok("Created");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")] //Post api/Account/Login 
        public async Task<IActionResult> Login([FromBody] LoginDto userFromRequest)
        {
            if (ModelState.IsValid)
            {
                User userFromDb = await userManager.FindByEmailAsync(userFromRequest.Email);

                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, userFromRequest.Password);

                    if (found)
                    {
                        // Generate token
                        var authClaims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, userFromDb.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Name, userFromDb.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, userFromDb.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token 
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // When the token was issued
                        };

                        // Get user roles
                        var userRoles = await userManager.GetRolesAsync(userFromDb);
                        foreach (var role in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                        // Design Toketn
                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: configuration["JWT:ValidIssuer"],
                            audience: configuration["JWT:ValidAudience"],
                            expires: DateTime.UtcNow.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                        return Ok(new
                        {
                            n = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
                ModelState.AddModelError("Email", "Invalid Email or Password");
            }
            return BadRequest(ModelState);
        }
    }
}
