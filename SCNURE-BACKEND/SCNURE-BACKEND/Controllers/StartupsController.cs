using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCNURE_BACKEND.Data.Entities.ClientEntities.Startup;
using SCNURE_BACKEND.Services.Users;

namespace SCNURE_BACKEND.Controllers
{
    [Route("api/startups")]
    [ApiController]
    public class StartupsController : ControllerBase
    {
        private readonly RemoteStartupMapper remoteStartupMapper = new RemoteStartupMapper(); 
        private readonly IStartupService startupService;
        public StartupsController(IStartupService _startupService)
        {
            startupService = _startupService;
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
        [HttpGet("startups")]
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
    }
}