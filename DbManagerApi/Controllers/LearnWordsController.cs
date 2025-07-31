using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Models;

namespace DbManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnWordsController : ControllerBase
    {
        private readonly SpellTestDbContext _context;

        public LearnWordsController(SpellTestDbContext context)
        {
            _context = context;
        }

        // GET: api/LearnWords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LearnWord>>> GetLearnWords()
        {
            return await _context.LearnWords.ToListAsync();
        }

        // GET: api/LearnWords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LearnWord>> GetLearnWord(int id)
        {
            var learnWord = await _context.LearnWords.FindAsync(id);

            if (learnWord == null)
            {
                return NotFound();
            }

            return learnWord;
        }

        // PUT: api/LearnWords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLearnWord(int id, LearnWord learnWord)
        {
            if (id != learnWord.Id)
            {
                return BadRequest();
            }

            _context.Entry(learnWord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LearnWordExists(id))
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

        // POST: api/LearnWords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LearnWord>> PostLearnWord(LearnWord learnWord)
        {
            _context.LearnWords.Add(learnWord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLearnWord", new { id = learnWord.Id }, learnWord);
        }

        // DELETE: api/LearnWords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLearnWord(int id)
        {
            var learnWord = await _context.LearnWords.FindAsync(id);
            if (learnWord == null)
            {
                return NotFound();
            }

            _context.LearnWords.Remove(learnWord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LearnWordExists(int id)
        {
            return _context.LearnWords.Any(e => e.Id == id);
        }
    }
}
