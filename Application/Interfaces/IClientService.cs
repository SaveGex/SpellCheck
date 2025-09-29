using Application.ModelsDTO;

namespace Application.Interfaces;

public interface IClientService
{
    Task<ClientResponseDTO> AddClientAsync(ClientRequestDTO clientRequest);
    Task<IEnumerable<ClientResponseDTO>> GetAllClientsAsync();
    Task<ClientResponseDTO> GetClientByIdAsync(string clientId);
    Task<ClientResponseDTO> UpdateClientByIdAsync(int clientId, ClientRequestDTO clientRequest);

}
