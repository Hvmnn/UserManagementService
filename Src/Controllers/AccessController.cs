using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Src.DTOs.Auth;
using UserManagementService.Src.Services;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly IAccessManagementService _accessManagementService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public AccessController(IAccessManagementService accessManagementService, IHttpContextAccessor httpContextAccessor, IRabbitMQProducer rabbitMQProducer)
        {
            _accessManagementService = accessManagementService;
            _httpContextAccessor = httpContextAccessor;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                await _accessManagementService.RegisterUserAsync(registerUserDto);

                var eventMessage = JsonSerializer.Serialize(new
                {
                    Event = "UserCreated",
                    Email = registerUserDto.Email,
                    RUT = registerUserDto.RUT,
                    FullName = $"{registerUserDto.Name} {registerUserDto.FirstLastName} {registerUserDto.SecondLastName}",
                    CareerId = registerUserDto.CareerId
                });

                _rabbitMQProducer.PublishUserCreated(eventMessage);

                return Ok(new { Message = "Usuario registrado exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error interno al registrar el usuario.", Details = ex.Message });
            }
        }


        [Authorize]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                var userEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized(new { Error = "Usuario no autenticado." });

                await _accessManagementService.UpdatePasswordAsync(userEmail, updatePasswordDto);
                return Ok(new { Message = "Contraseña actualizada exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

}