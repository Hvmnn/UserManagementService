using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Src.Models
{
    public class Career : BaseModel
    {
        [StringLength(250)]
        public string Name { get; set; } = null!;
    }
}