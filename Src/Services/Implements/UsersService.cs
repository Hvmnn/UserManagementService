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
        private readonly IAuthService _authService;

        public UsersService(IMapperService mapperService, IUsersRepository usersRepository, IAuthService authService)
        {
            _mapperService = mapperService;
            _usersRepository = usersRepository;
            _authService = authService;
        }
        public async Task<UserDto> EditProfile(EditProfileDto editProfileDto)
        {
            var userEmail = _authService.GetUserEmailInToken();
            var user = await GetUserByEmail(userEmail);

            user.Name = editProfileDto.Name;
            user.FirstLastName = editProfileDto.FirstLastName;
            user.SecondLastName = editProfileDto.SecondLastName;

            var updatedUser = await _usersRepository.UpdateUserAsync(user);

            return _mapperService.Map<User, UserDto>(updatedUser);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _usersRepository.GetAllAsync();
            return _mapperService.MapList<User, UserDto>(users);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await GetUserByEmail(email);
            return _mapperService.Map<User?, UserDto>(user);
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await GetUserById(id);
            return _mapperService.Map<User, UserDto>(user);
        }

        public Task<UserDto> GetProfile()
        {
            var userEmail = _authService.GetUserEmailInToken();
            return GetByEmail(userEmail);
        }

        public async Task<List<UserProgressDto>> GetUserProgress()
        {
            var userId = await GetUserIdByToken();
            var userProgress = await _usersRepository.GetProgressByUserAsync(userId) ?? new List <UserProgress>();
            var mappedProgress = userProgress.Select(up => new UserProgressDto()
            {
                Id = up.Id,
                SubjectCode = up.Subject.Code,
            }).ToList();

            return mappedProgress;
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

        private async Task<User> GetUserByEmail(string email)
        {
            var user = await _usersRepository.GetUserByEmailAsync(email)
                                        ?? throw new InvalidOperationException($"User with email: {email} not found");
            return user;
        }

        private async Task<User> GetUserById(int id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id)
                                        ?? throw new InvalidOperationException($"User with id: {id} not found");
            return user;
        }

        private async Task<int> GetUserIdByToken()
        {
            var userEmail = _authService.GetUserEmailInToken();
            var user = await _usersRepository.GetUserByEmailAsync(userEmail) ??
                          throw new InvalidOperationException("User not found");
            return user.Id;
        }
    }
}