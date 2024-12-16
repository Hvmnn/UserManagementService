using UserManagementService.Src.DTOs;
using UserManagementService.Src.Exceptions;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Services.Implements
{
    public class UsersService : IUsersService
    {
        private readonly IMapperService _mapperService;
        private readonly IUsersRepository _usersRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IAuthService _authService;

        public UsersService(IMapperService mapperService, IUsersRepository usersRepository, IAuthService authService, ISubjectsRepository subjectsRepository)
        {
            _mapperService = mapperService;
            _usersRepository = usersRepository;
            _authService = authService;
            _subjectsRepository = subjectsRepository;
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

        public async Task SetUserProgress(UpdateUserProgressDto subjects)
        {

            if ((subjects.AddSubjects == null || !subjects.AddSubjects.Any()) &&
                (subjects.DeleteSubjects == null || !subjects.DeleteSubjects.Any()))
            {
                return;
            }

            var subjectsId = await MapAndValidateToSubjectId(subjects);
            var subjectsToAdd = subjectsId.Item1;
            var subjectsToDelete = subjectsId.Item2;

            var userId = await GetUserIdByToken();
            var userProgress = await _usersRepository.GetProgressByUserAsync(userId) ?? new List<UserProgress>();

            var progressToAdd = subjectsToAdd.Select(s =>
            {
                var foundUserProgress = userProgress.FirstOrDefault(up => up.SubjectId == s);

                if (foundUserProgress is not null)
                    throw new DuplicateEntityException($"Subject with ID: {foundUserProgress.Subject.Code} already exists");

                return new UserProgress()
                {
                    SubjectId = s,
                    UserId = userId,
                };
            }).ToList();

            var progressToRemove = subjectsToDelete.Select(s =>
            {
                if (userProgress.FirstOrDefault(up => up.SubjectId == s) is null)
                    throw new EntityNotFoundException($"Subject with ID: {s} not found");

                return new UserProgress()
                {
                    SubjectId = s,
                    UserId = userId,
                };
            }).ToList();

            var addResult = progressToAdd.Any() ? await _usersRepository.AddProgressAsync(progressToAdd) : false;
            var removeResult = progressToRemove.Any() ? await _usersRepository.DeleteProgressAsync(progressToRemove, userId) : false;

            if (!removeResult && !addResult)
                throw new InternalErrorException("Cannot update user progress");
        }





        private async Task<User> GetUserByEmail(string email)
        {
            var user = await _usersRepository.GetUserByEmailAsync(email)
                                        ?? throw new EntityNotFoundException($"User with email: {email} not found");
            return user;
        }

        private async Task<User> GetUserById(int id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id)
                                        ?? throw new EntityNotFoundException($"User with id: {id} not found");
            return user;
        }

        private async Task<int> GetUserIdByToken()
        {
            var userEmail = _authService.GetUserEmailInToken();
            var user = await _usersRepository.GetUserByEmailAsync(userEmail) ??
                          throw new EntityNotFoundException("User not found");
            return user.Id;
        }

        private async Task<Tuple<List<int>, List<int>>> MapAndValidateToSubjectId(UpdateUserProgressDto subjects)
        {
            var allSubjects = await _subjectsRepository.GetAllSubjectsAsync();
            var subjectsToAdd = subjects.AddSubjects;
            var subjectsToDelete = subjects.DeleteSubjects;

            var mappedSubjectsToAdd = subjectsToAdd.Select(s =>
            {
                s = s.ToLower();
                var foundSubject = allSubjects.FirstOrDefault(sub => sub.Code.Equals(s, StringComparison.OrdinalIgnoreCase))
                    ?? throw new EntityNotFoundException($"Subject with ID: {s} not found");
                return foundSubject.Id;
            }).ToList();

            var mappedSubjectsToDelete = subjectsToDelete.Select(s =>
            {
                s = s.ToLower();
                var foundSubject = allSubjects.FirstOrDefault(sub => sub.Code.Equals(s, StringComparison.OrdinalIgnoreCase))
                    ?? throw new EntityNotFoundException($"Subject with ID: {s} not found");
                return foundSubject.Id;
            }).ToList();

            return new Tuple<List<int>, List<int>>(mappedSubjectsToAdd, mappedSubjectsToDelete);

        }
    }
}