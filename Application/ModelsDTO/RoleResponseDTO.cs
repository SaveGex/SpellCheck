using DomainData.Models;
using System.ComponentModel;

namespace Application.ModelsDTO
{
    public class RoleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
