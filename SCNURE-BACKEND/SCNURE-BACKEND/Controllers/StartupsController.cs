using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data.Dtos.Canvases;
using SCNURE_BACKEND.Data.Dtos.Comments;
using SCNURE_BACKEND.Data.Dtos.Mappers;
using SCNURE_BACKEND.Data.Dtos.Startups;
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
        private readonly RemoteTeamMemberMapper remoteTeamMemberMapper = new RemoteTeamMemberMapper();
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
				int likesCount = remoteStartup.Likes.Where(l => l.Value == LikeType.Like).Count();
				int dislikesCount = remoteStartup.Likes.Where(l => l.Value == LikeType.Dislike).Count();

				int currentUserLikeInt = LikeType.NoRateInt;

				if (HttpContext.User.Identity.Name != null)
				{
					int contextUserId = int.Parse(HttpContext.User.Identity.Name);

					var currentUserLike = remoteStartup.Likes.SingleOrDefault(l => l.StartupId == remoteStartup.StartupId && l.UserId == contextUserId);
					currentUserLikeInt = currentUserLike == null ? LikeType.NoRateInt : (currentUserLike.Value == LikeType.Like ? LikeType.LikeInt : LikeType.DislikeInt);
				}

				var clientResult = remoteStartupMapper.MapRemoteStartup(remoteStartup, likesCount, dislikesCount, currentUserLikeInt);

				return Ok(clientResult);
            }
            catch(Exception exception)
			{
				return BadRequest(new { message = exception.Message });
			}
        }

		[AllowAnonymous]
		[HttpGet("canvase")]
		public async Task<IActionResult> GetCanvaseAsync([Required]int startupId)
		{
			try
			{
				var canvase = await startupService.GetCanvaseById(startupId);

				return Ok(canvase);
			}
			catch (Exception exception)
			{
				return BadRequest(new { message = exception.Message });
			}
		}

        [AllowAnonymous]
        [HttpGet("team-members")]
        public async Task<IActionResult> GetStartupTeamMembers([Required]int startupId)
        {
            try
            {
                var remoteTeamMembers = await startupService.GetTeamMembers(startupId);
                var mappedTeamMembers = remoteTeamMemberMapper.MapRemoteTeamMembers(remoteTeamMembers);
                return Ok(mappedTeamMembers);
            }
            catch (Exception exception)
			{
				return BadRequest(new { message = exception.Message });
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
				return BadRequest(new { message = exception.Message });
			}
        }

		[Authorize]
		[HttpGet("my-startups")]
		public async Task<IActionResult> GetMyStartups()
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);
				var startups = await startupService.GetMyStartups(contextUserId);
				var startupDtos = remoteStartupMapper.MapRemoteStartups(startups);
				return Ok(startupDtos);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[Authorize]
		[HttpPost("rate-startup")]
		public async Task<IActionResult> RateStartup(RateStartupDto dto)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);

				await startupService.RateStartup(dto, contextUserId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
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

				await startupService.AddTeamMember(addTeamMemberRequest);
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
                var clientAddeedStartup = remoteStartupMapper.MapRemoteStartup(remoteAddedStartup, 0, 0, 0);
                return Ok(clientAddeedStartup);
            }
            catch (Exception exception)
			{
				return BadRequest(new { message = exception.Message });
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
                return Ok(remoteStartupMapper.MapRemoteStartup(updatedStartup, 0, 0, 0));
            }
            catch (Exception exception)
			{
				return BadRequest(new { message = exception.Message });
			}
        }

		[Authorize]
		[HttpPost("update-canvase")]
		public async Task<IActionResult> UpdateCanvase(CanvaseUpdateDto dto)
		{
			try
			{
				var userId = int.Parse(HttpContext.User.Identity.Name);
				var user = await userService.GetByIdAsync(userId);
				var startup = await startupService.GetStartupById(dto.StartupId);

				if (user == null || startup == null)
					return NotFound();
				var teamMembers = await startupService.GetTeamMembers(startup.StartupId);
				var userAsTeamMember = teamMembers.FirstOrDefault(x => x.UserId == user.UserId);
				if (userAsTeamMember == null || !userAsTeamMember.EditAccess)
					return BadRequest("NO_EDIT_ACCESS");

				var updatedCanvase = await startupService.UpdateCanvase(dto);
				return Ok(updatedCanvase);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

        [Authorize]
        [HttpPost("edit-team-member")]
        public async Task<IActionResult> RemoveStartupTeamMember([Required]EditTeamMemberRequest requestBody)
        {
            try
            {
                int contextUserId = int.Parse(HttpContext.User.Identity.Name);
                var contextUser = await userService.GetByIdAsync(contextUserId);

                await startupService.EditTeamMember(requestBody, contextUser);

                return Ok();
            }
            catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
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

				await startupService.RemoveTeamMember(userId, startupId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[Authorize]
		[HttpDelete("remove-rate")]
		public async Task<IActionResult> RemoveRate([Required]int startupId)
		{
			try
			{
				int contextUserId = int.Parse(HttpContext.User.Identity.Name);

				await startupService.RemoveRate(startupId, contextUserId);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

        [Authorize]
        [HttpPut("add-comment")]
        public async Task<IActionResult> AddComment([Required] int startupId, [Required] string text) 
        {
            try
            {
                int contextUserId = int.Parse(HttpContext.User.Identity.Name);
                var response = await startupService.AddComment(startupId, contextUserId, text);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("comments-for-startup")]
        public async Task<IActionResult> GetCommentsForStartup([Required] int startupId)
        {
            try
            {
                var response = await startupService.GetAllComments(startupId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("remove-comment")]
        public async Task<IActionResult> RemoveComment([Required] int commentId)
        {
            try
            {
                int contextUserId = int.Parse(HttpContext.User.Identity.Name);
                await startupService.RemoveComment(contextUserId, commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
	}
}