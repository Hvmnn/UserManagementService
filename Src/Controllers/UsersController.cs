using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Src.DTOs;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<ActionResult<UserDto>> EditProfile([FromBody] EditProfileDto editProfileDto)
        {
            var user = await _usersService.EditProfile(editProfileDto);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var user = await _usersService.GetProfile();
            return Ok(user);
        }

        [Authorize]
        [HttpGet("my-progress")]
        public async Task<ActionResult<List<UserProgressDto>>> GetUserProgress()
        {
            var userProgress = await _usersService.GetUserProgress();
            return Ok(userProgress);
        }

        [Authorize]
        [HttpPatch("my-progress")]
        public async Task<ActionResult> SetUserProgress([FromBody] UpdateUserProgressDto subjects)
        {
            await _usersService.SetUserProgress(subjects);
            return NoContent();
        }
    }
}