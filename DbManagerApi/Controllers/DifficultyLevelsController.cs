using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Models;

namespace DbManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyLevelsController : ControllerBase
    {
        private readonly SpellTestDbContext _context;

        public DifficultyLevelsController(SpellTestDbContext context)
        {
            _context = context;
        }

        // GET: api/DifficultyLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DifficultyLevel>>> GetDifficultyLevels()
        {
            return await _context.DifficultyLevels.ToListAsync();
        }

        // GET: api/DifficultyLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DifficultyLevel>> GetDifficultyLevel(int id)
        {
            var difficultyLevel = await _context.DifficultyLevels.FindAsync(id);

            if (difficultyLevel == null)
            {
                return NotFound();
            }

            return difficultyLevel;
        }

        // PUT: api/DifficultyLevels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficultyLevel(int id, DifficultyLevel difficultyLevel)
        {
            if (id != difficultyLevel.Id)
            {
                return BadRequest();
            }

            _context.Entry(difficultyLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DifficultyLevelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DifficultyLevels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DifficultyLevel>> PostDifficultyLevel(DifficultyLevel difficultyLevel)
        {
            _context.DifficultyLevels.Add(difficultyLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDifficultyLevel", new { id = difficultyLevel.Id }, difficultyLevel);
        }

        // DELETE: api/DifficultyLevels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDifficultyLevel(int id)
        {
            var difficultyLevel = await _context.DifficultyLevels.FindAsync(id);
            if (difficultyLevel == null)
            {
                return NotFound();
            }

            _context.DifficultyLevels.Remove(difficultyLevel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DifficultyLevelExists(int id)
        {
            return _context.DifficultyLevels.Any(e => e.Id == id);
        }
    }
}
