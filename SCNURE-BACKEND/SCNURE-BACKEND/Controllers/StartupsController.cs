using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Dtos.TeamMembers;
using SCNURE_BACKEND.Data.Entities.ClientEntities.Startup;
using SCNURE_BACKEND.Helpers;
using SCNURE_BACKEND.Services.Users;
using SCNURE_BACKEND.UseCases;

namespace SCNURE_BACKEND.Controllers
{
    [Route("api/startups")]
    [ApiController]
    public class StartupsController : ControllerBase
    {
        private readonly RemoteStartupMapper remoteStartupMapper = new RemoteStartupMapper(); 
        private readonly IStartupService startupService;
        private readonly IUserService userService;
        private readonly IOptions<JwtSettings> jwtSettings;

        public StartupsController(IStartupService _startupService, IOptions<JwtSettings> _jwtSettings, IUserService _userService)
        {
            startupService = _startupService;
            jwtSettings = _jwtSettings;
            userService = _userService;
        }

        [AllowAnonymous]
        [HttpGet("startup")]
        public async Task<IActionResult> GetStartupAsync([Required]int id)
        {
            try
            {
                var remoteStartup = await startupService.GetStartupById(id);
                var clientResult = remoteStartupMapper.MapRemoteStartup(remoteStartup);
                return Ok(clientResult);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("team-members")]
        public async Task<IActionResult> GetStartupTeamMembers([Required]int startupId)
        {
            try
            {
                //TODO: map response
                var teamMembers = await startupService.GetTeamMembers(startupId);
                return Ok(teamMembers);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("all-startups")]
        public async Task<IActionResult> GetAllStartupsAsync()
        {
            try
            {
                var remoteStartups = await startupService.GetAllStartups();
                var clientResult = remoteStartupMapper.MapRemoteStartups(remoteStartups);
                return Ok(clientResult);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

		[Authorize]
		[HttpPost("add-team-member")]
		public async Task<IActionResult> AddStartupTeamMembers(AddTeamMemberRequest addTeamMemberRequest)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);

				var user = await userService.GetByIdAsync(contextUserId);
				if (!await userService.HasEditAccess(user.UserId, addTeamMemberRequest.StartupId))
					return BadRequest(new { message = "Current user can't add team members" });

				await userService.AddTeamMember(addTeamMemberRequest);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[Authorize]
        [HttpPost("create-startup")]
        public async Task<IActionResult> CreateStartup([Required]CreateStartupDto requestBody)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                var user = await userService.GetByIdAsync(userId);

                if (!user.Membership)
                {
                    return BadRequest("NOT_MEMBER");
                }

                var remoteAddedStartup = await startupService.AddStartup(requestBody.Title, requestBody.Description, user);
                var clientAddeedStartup = remoteStartupMapper.MapRemoteStartup(remoteAddedStartup);
                return Ok(clientAddeedStartup);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Authorize]
        [HttpPost("edit-startup")]
        public async Task<IActionResult> EditStartup([Required] EditStartupDto requestBody)
        {
            try
            {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                var user = await userService.GetByIdAsync(userId);
                var startup = await startupService.GetStartupById(requestBody.StartupId);

                if (user == null || startup == null)
                {
                    return NotFound();
                }
                var teamMembers = await startupService.GetTeamMembers(startup.StartupId);
                var userAsTeamMember = teamMembers.FirstOrDefault (x => x.UserId == user.UserId);
                if (userAsTeamMember == null || !userAsTeamMember.EditAccess)
                {
                    return BadRequest("NO_EDIT_ACCESS");
                }

                if (requestBody.Title != null)
                {
                    startup.Title = requestBody.Title;
                }
                if (requestBody.Description != null)
                {
                    startup.Description = requestBody.Description;
                }
                if (requestBody.Phone != null)
                {
                    if (!IsValidPhoneUC.IsValidPhone(requestBody.Phone))
                    {
                        return BadRequest("INVALID_PHONE");
                    }
                    startup.Phone = requestBody.Phone;
                }
                if (requestBody.Email != null)
                {
                    if (!IsValidEmailUC.IsValidEmail(requestBody.Email))
                    {
                        return BadRequest("INVALID_EMAIL");
                    }
                    startup.Email = requestBody.Email;
                }
                if (requestBody.Website != null)
                {
                    startup.Website = requestBody.Website;
                }

                var updatedStartup = await startupService.UpdateStartup(startup);
                return Ok(remoteStartupMapper.MapRemoteStartup(updatedStartup));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

		[Authorize]
		[HttpDelete("remove-team-member")]
		public async Task<IActionResult> RemoveStartupTeamMember([Required]int userId, [Required]int startupId)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);

				var user = await userService.GetByIdAsync(contextUserId);
				if (!await userService.HasEditAccess(user.UserId, startupId))
					return BadRequest(new { message = "Current user can't add team members" });

				await userService.RemoveTeamMember(userId, startupId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}