using Microsoft.AspNetCore.Identity;
using UserManagementService.Src.DTOs.Auth;
using UserManagementService.Src.Models;
using UserManagementService.Src.Repositories.Interfaces;
using UserManagementService.Src.Services.Interfaces;

namespace UserManagementService.Src.Services.Implements
{
    public class AccessManagementService : IAccessManagementService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICareersRepository _careersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccessManagementService(
            IUsersRepository userRepository,
            ICareersRepository careerRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _usersRepository = userRepository;
            _careersRepository = careerRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var existingRUT = await _usersRepository.GetUserByRut(registerUserDto.RUT);
            if (existingRUT != null)
                throw new ArgumentException("El rut proporcionado ya está registrado.");

            var career = await _careersRepository.GetByIdAsync(registerUserDto.CareerId);
            if (career == null)
                throw new ArgumentException("La carrera proporcionada no existe.");

            var existingUser = await _usersRepository.GetUserByEmailAsync(registerUserDto.Email);
            if (existingUser != null)
                throw new ArgumentException("El correo electrónico ya está registrado.");

            var user = new User
            {
                Name = registerUserDto.Name,
                FirstLastName = registerUserDto.FirstLastName,
                SecondLastName = registerUserDto.SecondLastName,
                RUT = registerUserDto.RUT,
                Email = registerUserDto.Email,
                CareerId = registerUserDto.CareerId,
                RoleId = 2,
                HashedPassword = _passwordHasher.HashPassword(null, registerUserDto.Password),
                IsEnabled = true
            };

            await _usersRepository.AddUserAsync(user);
        }

        public async Task UpdatePasswordAsync(string userEmail, UpdatePasswordDto updatePasswordDto)
        {
            var user = await _usersRepository.GetUserByEmailAsync(userEmail)
                    ?? throw new ArgumentException("Usuario no encontrado.");

            var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, updatePasswordDto.CurrentPassword);
            if (passwordVerification != PasswordVerificationResult.Success)
                throw new ArgumentException("La contraseña actual no es válida.");

            user.HashedPassword = _passwordHasher.HashPassword(user, updatePasswordDto.Password);
            await _usersRepository.UpdateUserAsync(user);
        }
    }
}