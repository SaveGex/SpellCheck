using Application.Interfaces;
using Application.ModelsDTO;
using DomainData.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(RoleNames.Admin)}")]
    public class ClientsController : ControllerBase
    {
        public IClientService ClientService { get; init; }

        public ClientsController(IClientService clientService)
        {
            ClientService = clientService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClientResponseDTO>>> GetAllClients()
        {
            IEnumerable<ClientResponseDTO> result;
            try
            {
                result = await ClientService.GetAllClientsAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ClientResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientResponseDTO>> CreateClient(
            [FromBody] ClientRequestDTO clientRequestDTO)
        {
            try
            {
                var result = await ClientService.AddClientAsync(clientRequestDTO);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{destId:int}")]
        [ProducesResponseType(typeof(ClientResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ClientResponseDTO>> UpdateClient(
            int destId,
            [FromBody] ClientRequestDTO clientRequestDTO)
        {
            ClientResponseDTO result;
            try
            {
                result = await ClientService.UpdateClientByIdAsync(destId, clientRequestDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }
    }
}
