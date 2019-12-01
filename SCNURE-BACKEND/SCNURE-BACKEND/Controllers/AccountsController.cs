using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SCNURE_BACKEND.Data.Dtos.Mappers;
using SCNURE_BACKEND.Data.Dtos.Users;
using SCNURE_BACKEND.Helpers;
using SCNURE_BACKEND.Services.Email;
using SCNURE_BACKEND.Services.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IUserService userService;
        private readonly JwtSettings jwtSettings;
		private readonly IEmailService emailService;

        public AccountsController(IUserService userService, IOptions<JwtSettings> jwtSettings,
			IEmailService emailService)
        {
            this.userService = userService;
            this.jwtSettings = jwtSettings.Value;
			this.emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest userDto)
        {
            var user = await userService.AuthenticateAsync(userDto.LoginOrEmail, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

			if (user.Ban)
				return BadRequest(new { message = "User is banned" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
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
                var user = await userService.RegisterAsync(userDto);
				if (user != null)
				{
					var callbackUrl = Url.Action(
						"confirmEmail",
						"accounts",
						new { token = user.Verification},
						protocol: HttpContext.Request.Scheme);
					await emailService.SendEmailAsync(user.Email, "Confirm your account",
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
				await userService.ConfirmUserEmailAsync(token);
				return Ok();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[AllowAnonymous]
		[HttpGet("profile")]
		public async Task<IActionResult> GetUserProfile(int? userId, string login)
		{
			try
			{
				if (userId.HasValue && !string.IsNullOrEmpty(login))
					return BadRequest(new { message = "Provide only login or only userId" });
				else if (userId.HasValue)
				{
					var user = await userService.GetUserProfile(userId.Value);
					return Ok(user);
				}
				else if (!string.IsNullOrEmpty(login))
				{
					var user = await userService.GetUserProfile(login);
					return Ok(user);
				}
				else
					return BadRequest(new { message = "No parameters" });
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

				var contextUser = await userService.GetByIdAsync(contextUserId);

				if (userId.HasValue && contextUserId != userId && contextUser.Admin)
				{
					var user = await userService.GetAccountData(userId.Value);
					return Ok(user);
				}
				else if (userId.HasValue && contextUserId != userId && !contextUser.Admin)
				{
					return BadRequest(new { message = "You have no permissions for that" });
				}
				else
				{
					var user = await userService.GetAccountData(contextUserId);
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
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);

				await userService.UpdateUser(user, contextUserId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetOwnProfile()
        {
            try
            {
                int contextUserId = int.Parse(HttpContext.User.Identity.Name);
                var user = await userService.GetByIdAsync(contextUserId);
                return Ok(user.ToUserDataResponse());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
