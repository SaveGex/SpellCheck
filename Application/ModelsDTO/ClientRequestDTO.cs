
using DomainData.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.ModelsDTO;

public class ClientRequestDTO
{
    [Required(ErrorMessage = "Client ID is required")]
    public string ClientId { get; set; } = null!;
    [Required(ErrorMessage = "Client Name is required")]
    [MaxLength(256, ErrorMessage = "Client Name cannot be longer than 256 characters.")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Client URL is required")]
    [MaxLength(512, ErrorMessage = "Client URL cannot be longer than 512 characters.")]
    public string URL { get; set; } = null!;

    private Guid Secret { get; set; } = Guid.NewGuid();
}
