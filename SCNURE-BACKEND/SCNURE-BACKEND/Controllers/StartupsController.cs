using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Entities.ClientEntities.Startup;
using SCNURE_BACKEND.Helpers;
using SCNURE_BACKEND.Services.Users;

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

        [AllowAnonymous]
        [HttpPost("create-startup")]
        public async Task<IActionResult> CreateStartup([Required]CreateStartupDto requestBody)
        {
            try
            {
                var user = await userService.GetUserByToken(requestBody.Token);
                if (user == null)
                {
                    return NotFound();
                }

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
    }
}