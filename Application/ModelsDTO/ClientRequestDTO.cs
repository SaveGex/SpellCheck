using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
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

    [JsonIgnore]
    public string Secret { get; private set; } = null!; // base64 encoded client secret

    public ClientRequestDTO() => Init();


    private void Init()
    {
        byte[] secretBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(secretBytes);
        Secret = Convert.ToBase64String(secretBytes);
    }
}
