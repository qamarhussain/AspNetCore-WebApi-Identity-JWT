using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreApiJwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreApiJwt.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> UserManager { get; }
        private SignInManager<IdentityUser> SignInManager { get; }

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = await UserManager.FindByNameAsync(model.Email);
                var result = await SignInManager.CheckPasswordSignInAsync(userInfo, model.Password,false);
                if (result.Succeeded)
                {
                    return Created("", CreateToken(userInfo));
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = new IdentityUser
                {   
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed=true
                };
                var result = await UserManager.CreateAsync(userInfo, model.Password);
                if (result.Succeeded)
                {
                    return Created("", CreateToken(userInfo));
                }
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }

        private object CreateToken(IdentityUser userInfo)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MicrosoftVSConstants.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub,userInfo.Email),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.Email)
                    };

            var token = new JwtSecurityToken(
                MicrosoftVSConstants.Issuer,
                MicrosoftVSConstants.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                 signingCredentials: cred
                );

            var response = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expireDate = token.ValidTo
            };

            return response;
        }
    }
}