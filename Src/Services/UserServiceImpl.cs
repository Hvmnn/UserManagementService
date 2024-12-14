using Grpc.Core;
using UserManagementService.Grpc;
using UserManagementService.Src.DTOs;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Services
{
    public class UserServiceImpl : UserService.UserServiceBase
    {
        private readonly IUsersService _usersService;

        public UserServiceImpl(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public override async Task<UserListResponse> GetAll(EmptyRequest request, ServerCallContext context)
        {
            var users = await _usersService.GetAll();
            var response = new UserListResponse();
            response.Users.AddRange(users.Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Email = user.Email,
                Rut = user.RUT,
                Career = new CareerResponse
                {
                    Id = user.Career.Id,
                    Name = user.Career.Name
                }
            }));
            return response;
        }

        public override async Task<UserResponse> GetByEmail(EmailRequest request, ServerCallContext context)
        {
            var user = await _usersService.GetByEmail(request.Email);
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Email = user.Email,
                Rut = user.RUT,
                Career = new CareerResponse
                {
                    Id = user.Career.Id,
                    Name = user.Career.Name
                }
            };
        }

        public override async Task<UserResponse> GetById(IdRequest request, ServerCallContext context)
        {
            var user = await _usersService.GetById(request.Id);
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Email = user.Email,
                Rut = user.RUT,
                Career = new CareerResponse
                {
                    Id = user.Career.Id,
                    Name = user.Career.Name
                }
            };
        }

        public override async Task<UserResponse> EditProfile(EditProfileRequest request, ServerCallContext context)
        {
            var updatedUser = await _usersService.EditProfile(new EditProfileDto
            {
                Name = request.Name,
                FirstLastName = request.FirstLastName,
                SecondLastName = request.SecondLastName
            });

            return new UserResponse
            {
                Id = updatedUser.Id,
                Name = updatedUser.Name,
                FirstLastName = updatedUser.FirstLastName,
                SecondLastName = updatedUser.SecondLastName,
                Email = updatedUser.Email,
                Rut = updatedUser.RUT,
                Career = new CareerResponse
                {
                    Id = updatedUser.Career.Id,
                    Name = updatedUser.Career.Name
                }
            };
        }

        public override async Task<IsEnabledResponse> IsEnabled(EmailRequest request, ServerCallContext context)
        {
            var isEnabled = await _usersService.IsEnabled(request.Email);
            return new IsEnabledResponse { IsEnabled = isEnabled };
        }

        public override async Task<UserResponse> GetProfile(EmptyRequest request, ServerCallContext context)
        {
            var user = await _usersService.GetProfile();
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Email = user.Email,
                Rut = user.RUT,
                Career = new CareerResponse
                {
                    Id = user.Career.Id,
                    Name = user.Career.Name
                }
            };
        }

        public override async Task<UserProgressListResponse> GetUserProgress(EmptyRequest request, ServerCallContext context)
        {
            var progress = await _usersService.GetUserProgress();
            var response = new UserProgressListResponse();
            response.Progress.AddRange(progress.Select(p => new UserProgress
            {
                Id = p.Id,
                SubjectCode = p.SubjectCode
            }));

            return response;
        }

        public override async Task<EmptyResponse> SetUserProgress(UpdateUserProgressRequest request, ServerCallContext context)
        {
            await _usersService.SetUserProgress(new UpdateUserProgressDto
            {
                AddSubjects = request.AddSubjects.ToList(),
                DeleteSubjects = request.DeleteSubjects.ToList()
            });

            return new EmptyResponse();
        }
    }
}