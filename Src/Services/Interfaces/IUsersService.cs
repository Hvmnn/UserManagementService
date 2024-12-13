using UserManagementService.Src.DTOs;

namespace UserManagementService.Src.Services.Interfaces
{
    public interface IUsersService
    {
        public Task<List<UserDto>> GetAll();
        public Task<UserDto> GetByEmail(string email);
        public Task<UserDto> GetById(int id);
        public Task<UserDto> EditProfile(EditProfileDto editProfileDto);
        public Task<bool> IsEnabled(string email);
        public Task<UserDto> GetProfile();
        public Task<List<UserProgressDto>> GetUserProgress();
        public Task SetUserProgress(UpdateUserProgressDto subjects);
    }
}