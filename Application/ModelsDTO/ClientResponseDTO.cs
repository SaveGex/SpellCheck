using DomainData.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

public class ClientResponseDTO
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Client ID is required")]
    public string ClientId { get; set; } = null!;
    [Required(ErrorMessage = "Client Name is required")]
    [MaxLength(256, ErrorMessage = "Client Name cannot be longer than 256 characters.")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Client Secret is required")]
    [MaxLength(512, ErrorMessage = "Client Secret cannot be longer than 512 characters.")]
    public Guid Secret { get; set; }
    [Required(ErrorMessage = "Client URL is required")]
    [MaxLength(512, ErrorMessage = "Client URL cannot be longer than 512 characters.")]
    public string URL { get; set; } = null!;
    [Required]
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
