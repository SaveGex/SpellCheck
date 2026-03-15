using Application.Interfaces;
using Application.ModelsDTO;
using DomainData.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = $"{nameof(RoleNames.Manager)}, {nameof(RoleNames.Admin)}")]
    public class DifficultyLevelsController : ControllerBase
    {
        IDifficultyLevelService DifficultyLevelService { get; init; }

        public DifficultyLevelsController(IDifficultyLevelService difficultyLevelService)
        {
            DifficultyLevelService = difficultyLevelService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DifficultyLevelResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DifficultyLevelResponseDTO>> CreateDifficultyLevel(
            [FromBody] DifficultyLevelCreateDTO dto)
        {
            try
            {
                var result = await DifficultyLevelService.CreateLevelAsync(dto);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DifficultyLevelResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DifficultyLevelResponseDTO>>> GetDifficultyLevels()
        {
            IEnumerable<DifficultyLevelResponseDTO> result;
            try
            {
                result = await DifficultyLevelService.GetLevelsAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }


        [HttpPut]
        [ProducesResponseType(typeof(DifficultyLevelResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<DifficultyLevelResponseDTO>> UpdateLevelAsync(
            [FromBody] DifficultyLevelUpdateDTO dto)
        {
            DifficultyLevelResponseDTO result;
            try
            {
                result = await DifficultyLevelService.UpdateLevelAsync(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }


        [HttpDelete("{levelId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLevelAsync(int levelId)
        {
            try
            {
                await DifficultyLevelService.DeleteLevelAsync(levelId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
