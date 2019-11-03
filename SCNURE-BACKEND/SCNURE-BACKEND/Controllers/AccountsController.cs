﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SCNURE_BACKEND.Data;
using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Dtos.Users;
using SCNURE_BACKEND.Data.Entities;
using SCNURE_BACKEND.Helpers;
using SCNURE_BACKEND.Services.Email;
using SCNURE_BACKEND.Services.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Controllers
{
	[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;
		private readonly IEmailService _emailService;

        public AccountsController(IUserService userService, IOptions<JwtSettings> jwtSettings,
			IEmailService emailService)
        {
            _userService = userService;
            _jwtSettings = jwtSettings.Value;
			_emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest userDto)
        {
            var user = await _userService.AuthenticateAsync(userDto.LoginOrEmail, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

			if (user.Ban)
				return BadRequest(new { message = "User is banned" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.UserId,
                Login = user.Login,
                Token = tokenString,
				IsAdmin = user.Admin
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest userDto)
        {
            try
            {
                var user = await _userService.RegisterAsync(userDto);
				if (user != null)
				{
					var callbackUrl = Url.Action(
						"confirmEmail",
						"accounts",
						new { token = user.Verification},
						protocol: HttpContext.Request.Scheme);
					await _emailService.SendEmailAsync(user.Email, "Confirm your account",
						$"<h3>Thanks for signing up to YEP! Startup Club</h3>" +
							$"<p>Please confirm your email address to complete your SCNURE registration.</p>" +
							$"<a href='{callbackUrl}'>Confirm your email</a>");
					return Ok();
				}
				else
				{
					return BadRequest(new { message = "User was not created" });
				}
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

		[AllowAnonymous]
		[HttpGet("confirmEmail")]
		public async Task<IActionResult> ConfirmEmail([Required]string token)
		{
			try
			{
				await _userService.ConfirmUserEmailAsync(token);
				return Ok();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[AllowAnonymous]
		[HttpGet("profile")]
		public async Task<IActionResult> GetUserProfile([Required]int userId)
		{
			try
			{
				var user = await _userService.GetUserProfile(userId);
				return Ok(user);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("accountData")]
		public async Task<IActionResult> GetAccountData(int? userId)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				if (contextUserId == 0)
					return BadRequest(new { message = "Unathorized" });

				var contextUser = await _userService.GetByIdAsync(contextUserId);

				if (userId.HasValue && contextUserId != userId && contextUser.Admin)
				{
					var user = await _userService.GetAccountData(userId.Value);
					return Ok(user);
				}
				else if (userId.HasValue && contextUserId != userId && !contextUser.Admin)
				{
					return BadRequest(new { message = "You have no permissions for that" });
				}
				else
				{
					var user = await _userService.GetAccountData(contextUserId);
					return Ok(user);
				}
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[Authorize]
 		[HttpPut]
		public async Task<IActionResult> EditUser([FromBody]EditUserDataRequest user)
		{
			try
			{
				await _userService.UpdateUser(user);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
    }
}