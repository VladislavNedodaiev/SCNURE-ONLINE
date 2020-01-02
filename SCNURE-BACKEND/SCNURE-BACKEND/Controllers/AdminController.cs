using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCNURE_BACKEND.Data.Dtos.Users;
using SCNURE_BACKEND.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IUserService usersService;

		public AdminController(IUserService usersService)
		{
			this.usersService = usersService;
		}

		[HttpGet("all-users")]
		public async Task<IActionResult> GetAllUsers()
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				if (contextUserId == 0)
					return BadRequest(new { message = "Unathorized" });

				bool isContextUserAdmin = await usersService.IsUserAdmin(contextUserId);
				if (!isContextUserAdmin)
					return BadRequest(new { message = "Current user isn't admin" });

				return Ok(await usersService.GetAllUsers());
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("membership-requests")]
		public async Task<IActionResult> GetAllMembershipRequests()
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				if (contextUserId == 0)
					return BadRequest(new { message = "Unathorized" });

				bool isContextUserAdmin = await usersService.IsUserAdmin(contextUserId);
				if (!isContextUserAdmin)
					return BadRequest(new { message = "Current user isn't admin" });

				return Ok(await usersService.GetMembershipRequests());
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("ban")]
		public async Task<IActionResult> BanUser(BanUserRequest dto)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				if (contextUserId == 0)
					return BadRequest(new { message = "Unathorized" });

				bool isContextUserAdmin = await usersService.IsUserAdmin(contextUserId);
				if (!isContextUserAdmin)
					return BadRequest(new { message = "Current user isn't admin" });

				await usersService.BanUser(dto.UserId);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("set-membership")]
		public async Task<IActionResult> SetMembership(BanUserRequest dto)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				if (contextUserId == 0)
					return BadRequest(new { message = "Unathorized" });

				bool isContextUserAdmin = await usersService.IsUserAdmin(contextUserId);
				if (!isContextUserAdmin)
					return BadRequest(new { message = "Current user isn't admin" });

				await usersService.SetMembership(dto.UserId);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("remove-membership")]
		public async Task<IActionResult> RemoveMembership(BanUserRequest dto)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				if (contextUserId == 0)
					return BadRequest(new { message = "Unathorized" });

				bool isContextUserAdmin = await usersService.IsUserAdmin(contextUserId);
				if (!isContextUserAdmin)
					return BadRequest(new { message = "Current user isn't admin" });

				await usersService.RemoveMembership(dto.UserId);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
