using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;

namespace Application.Services;

public class ClientService : IClientService
{
    public IClientRepository ClientRepository { get; init; }
    public IMapper Mapper { get; init; }

    public ClientService(IClientRepository clientRepository, IMapper mapper)
    {
        ClientRepository = clientRepository;
        Mapper = mapper;
    }

    public async Task<ClientResponseDTO> AddClientAsync(ClientRequestDTO clientRequest)
    {
        Client addedClient = await ClientRepository.AddClientAsync(
            Mapper.Map<Client>(clientRequest));
        return Mapper.Map<ClientResponseDTO>(addedClient);
    }

    public async Task<IEnumerable<ClientResponseDTO>> GetAllClientsAsync()
    {
        List<ClientResponseDTO> result = new List<ClientResponseDTO>();
        foreach(var item in await ClientRepository.GetAllClientsAsync())
        {
            result.Add(
                Mapper.Map<ClientResponseDTO>(item));
        }
        return result;
    }

    public async Task<ClientResponseDTO> GetClientByIdAsync(string clientId)
    {
        return Mapper.Map<ClientResponseDTO>(
            await ClientRepository.GetClientByClientIdAsync(clientId));
    }

    public async Task<ClientResponseDTO> UpdateClientByIdAsync(int clientId, ClientRequestDTO clientRequest)
    {
        ClientResponseDTO result = Mapper.Map<ClientResponseDTO>(
            await ClientRepository.UpdateClientAsync(clientId, Mapper.Map<Client>(clientRequest)));
        return result;
    }
}
