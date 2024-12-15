using UserManagementService.Src.DTOs.Auth;

namespace UserManagementService.Src.Services.Interfaces
{
    public interface IAccessManagementService
    {
        Task RegisterUserAsync(RegisterUserDto registerUserDto);
        Task UpdatePasswordAsync(string userEmail, UpdatePasswordDto updatePasswordDto);
    }
}