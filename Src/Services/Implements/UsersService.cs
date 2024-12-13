using UserManagementService.Src.DTOs;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Services.Implements
{
    public class UsersService : IUsersService
    {
        private readonly IMapperService _mapperService;
        private readonly IUsersRepository _usersRepository;

        public UsersService(IMapperService mapperService, IUsersRepository usersRepository)
        {
            _mapperService = mapperService;
            _usersRepository = usersRepository;
        }
        public async Task<UserDto> EditProfile(EditProfileDto editProfileDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _usersRepository.GetAllAsync();
            return _mapperService.MapList<User, UserDto>(users);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await _usersRepository.GetUserByEmailAsync(email);
            return _mapperService.Map<User?, UserDto>(user);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id) ?? throw new Exception("User not found");
            return _mapperService.Map<User, UserDto>(user);
        }

        public Task<UserDto> GetProfile()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserProgressDto>> GetUserProgress()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEnabled(string email)
        {
            try
            {
                var user = await _usersRepository.GetUserByEmailAsync(email);
                if (user == null || !user.IsEnabled)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task SetUserProgress(UpdateUserProgressDto subjects)
        {
            throw new NotImplementedException();
        }

    }
}