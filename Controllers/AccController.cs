using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using BookMgtApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookMgtApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AccController : ControllerBase
    {
        private readonly UserManager<User> _accountManager;
        private readonly IConfiguration _configuration;

        public AccController(UserManager<User> accountManager, IConfiguration configuration)
        {
            _accountManager = accountManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]Register userData)
        {
            var dbUser = await _accountManager.FindByEmailAsync(userData.Email);
            if(dbUser != null)
            {
                return UnprocessableEntity("Email already exists");
            }

            User user = new User() 
            {
                Email = userData.Email,
                UserName = userData.UserName
            };

            var result =  await _accountManager.CreateAsync(user, userData.Password);
            if(!result.Succeeded) return UnprocessableEntity("failed to create account");
            
            return Created("Acount added", new {email = user.Email, user = user.UserName});
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login userData)
        {
            var dbUser = await _accountManager.FindByEmailAsync(userData.Email);
            if(dbUser != null && await _accountManager.CheckPasswordAsync(dbUser, userData.Password))
            {
                var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires = token.ValidTo
                });
            }
            return Unauthorized("Invalid credentials");
        }
        
    }
}