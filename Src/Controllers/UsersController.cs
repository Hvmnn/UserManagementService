using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Src.DTOs;
using UserManagementService.Src.Exceptions;
using UserManagementService.Src.Services;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public UsersController(IUsersService usersService, IRabbitMQProducer rabbitMQProducer)
        {
            _usersService = usersService;
            _rabbitMQProducer = rabbitMQProducer;
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
            try
            {
                await _usersService.SetUserProgress(subjects);

                var eventMessage = JsonSerializer.Serialize(new
                {
                    Event = "ProgressUpdated",
                    SubjectsAdded = subjects.AddSubjects,
                    SubjectsDeleted = subjects.DeleteSubjects
                });

                _rabbitMQProducer.PublishProgressUpdated(eventMessage);

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error interno al actualizar el progreso.", Details = ex.Message });
            }
        }

    }
}