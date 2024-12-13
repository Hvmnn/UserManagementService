namespace UserManagementService.Src.DTOs
{
    public class UserDto : BaseModelDto
    {
        public string Name { get; set; } = null!;

        public string FirstLastName { get; set; } = null!;

        public string SecondLastName { get; set; } = null!;

        public string RUT { get; set; } = null!;

        public string Email { get; set; } = null!;

        public CareerDto Career { get; set; } = null!;

    }
}