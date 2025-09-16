using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO
{
    public class RoleCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}