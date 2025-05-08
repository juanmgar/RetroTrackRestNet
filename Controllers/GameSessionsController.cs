using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroTrackRestNet.Data;
using RetroTrackRestNet.DTO;
using RetroTrackRestNet.Model;

namespace RetroTrackRestNet.Controllers
{
    [Route("/rest/api/[controller]")]
    [ApiController]
    public class GameSessionsController : ControllerBase
    {
        private readonly DataContext _context;

        public GameSessionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/GameSessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameSession>>> GetGameSessions()
        {
            return await _context.GameSessions.ToListAsync();
        }

        // GET: api/GameSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameSession>> GetGameSession(int id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);

            if (gameSession == null)
            {
                return NotFound();
            }

            return gameSession;
        }

        [HttpGet("{id}/screenshot")]
        public async Task<IActionResult> GetGameSessionScreenshot(int id)
        {
            var session = await _context.GameSessions.FindAsync(id);

            if (session == null || session.Screenshot == null)
            {
                return NotFound();
            }

            return File(session.Screenshot, "image/png");
        }


        // PUT: api/GameSessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameSession(int id, GameSession gameSession)
        {
            if (id != gameSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameSessionExists(id))
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

        // POST: api/GameSessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameSession>> PostGameSession([FromForm] GameSessionDto dto)
        {
            byte[] screenshotBytes = null;
            if (dto.Screenshot != null)
            {
                using (var ms = new MemoryStream())
                {
                    await dto.Screenshot.CopyToAsync(ms);
                    screenshotBytes = ms.ToArray();
                }
            }

            var gameSession = new GameSession
            {
                PlayerId = dto.PlayerId,
                GameId = dto.GameId,
                PlayedAt = dto.PlayedAt,
                MinutesPlayed = dto.MinutesPlayed,
                Screenshot = screenshotBytes
            };

            _context.GameSessions.Add(gameSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameSession", new { id = gameSession.Id }, gameSession);
        }


        // DELETE: api/GameSessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameSession(int id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);
            if (gameSession == null)
            {
                return NotFound();
            }

            _context.GameSessions.Remove(gameSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameSessionExists(int id)
        {
            return _context.GameSessions.Any(e => e.Id == id);
        }
    }
}
