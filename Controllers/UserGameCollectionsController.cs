using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroTrackRestNet.Data;
using RetroTrackRestNet.Model;

namespace RetroTrackRestNet.Controllers
{
    [Authorize]
    [Route("/retrotrack/api/[controller]")]
    [ApiController]
    public class UserGameCollectionsController : ControllerBase
    {
        private readonly DataContext _context;

        public UserGameCollectionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/UserGameCollections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGameCollection>>> GetUserGameCollections()
        {
            return await _context.UserGameCollections.ToListAsync();
        }

        // GET: api/UserGameCollections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGameCollection>> GetUserGameCollection(int id)
        {
            var userGameCollection = await _context.UserGameCollections.FindAsync(id);

            if (userGameCollection == null)
            {
                return NotFound();
            }

            return userGameCollection;
        }

        // PUT: api/UserGameCollections/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserGameCollection(int id, UserGameCollection userGameCollection)
        {
            if (id != userGameCollection.Id)
            {
                return BadRequest();
            }

            _context.Entry(userGameCollection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGameCollectionExists(id))
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

        // POST: api/UserGameCollections
        [HttpPost]
        public async Task<ActionResult<UserGameCollection>> PostUserGameCollection(UserGameCollection userGameCollection)
        {
            _context.UserGameCollections.Add(userGameCollection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserGameCollection", new { id = userGameCollection.Id }, userGameCollection);
        }

        // DELETE: api/UserGameCollections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGameCollection(int id)
        {
            var userGameCollection = await _context.UserGameCollections.FindAsync(id);
            if (userGameCollection == null)
            {
                return NotFound();
            }

            _context.UserGameCollections.Remove(userGameCollection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/UserGameCollections/user/{username}
        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<UserGameCollection>>> GetUserGameCollectionsByUser(string username)
        {
            var userCollections = await _context.UserGameCollections
                                                .Where(c => c.User == username)
                                                .ToListAsync();

            return Ok(userCollections);
        }

        private bool UserGameCollectionExists(int id)
        {
            return _context.UserGameCollections.Any(e => e.Id == id);
        }
    }
}
