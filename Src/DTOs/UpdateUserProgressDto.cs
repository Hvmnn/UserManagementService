namespace UserManagementService.Src.DTOs
{
    public class UpdateUserProgressDto
    {
        public List<string> AddSubjects { get; set; } = new();

        public List<string> DeleteSubjects { get; set; } = new();
    }
}