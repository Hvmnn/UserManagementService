using System.ComponentModel.DataAnnotations;
using UserManagementService.Src.Validators;
using UserManagementService.Src.Validators.Constants;

namespace UserManagementService.Src.DTOs.Auth
{
        public class RegisterUserDto
        {
            [Required]
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; } = null!;

            [Required]
            [StringLength(30, MinimumLength = 3)]
            public string FirstLastName { get; set; } = null!;

            [Required]
            [StringLength(30, MinimumLength = 3)]
            public string SecondLastName { get; set; } = null!;

            [Required]
            [Rut]
            public string RUT { get; set; } = null!;

            [Required]
            [UCNEmailAddress("Email must be from Universidad Catolica del Norte (UCN)")]
            public string Email { get; set; } = null!;

            [Required]
            public int CareerId { get; set; }

            [Required]
            [StringLength(16, MinimumLength = 10)]
            [RegularExpression(RegularExpressions.PasswordValidation,
                ErrorMessage = "Password must have at least one letter and one number")
            ]
            public string Password { get; set; } = null!;

            [Required]
            [Compare(nameof(Password))]
            public string RepeatedPassword { get; set; } = null!;
        }
}