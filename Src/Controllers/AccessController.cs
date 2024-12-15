using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Src.DTOs.Auth;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly IAccessManagementService _accessManagementService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessController(IAccessManagementService accessManagementService, IHttpContextAccessor httpContextAccessor)
        {
            _accessManagementService = accessManagementService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                await _accessManagementService.RegisterUserAsync(registerUserDto);
                return Ok(new { Message = "Usuario registrado exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
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