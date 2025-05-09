using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroTrackRestNet.Data;
using RetroTrackRestNet.Model;

namespace RetroTrackRestNet.Controllers
{
    [Route("/rest/api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DataContext _context;

        public GamesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return await _context.Games.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
                return BadRequest();

            await EnrichGameWithExternalData(game);

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            await EnrichGameWithExternalData(game);

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }


        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }

        private async Task EnrichGameWithExternalData(Game game)
        {
            var codeID = "e6a0126080a14743825d61ecc3e5a349";
            var rawgUrl = $"https://api.rawg.io/api/games?search={Uri.EscapeDataString(game.Title)}&key={codeID}";

            using var client = new HttpClient();
            var response = await client.GetAsync(rawgUrl);

            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
            {
                var firstGame = results[0];
                if (firstGame.TryGetProperty("slug", out var slug))
                {
                    var detailUrl = $"https://api.rawg.io/api/games/{slug.GetString()}?key={codeID}";
                    var detailResponse = await client.GetAsync(detailUrl);

                    if (detailResponse.IsSuccessStatusCode)
                    {
                        var detailJson = await detailResponse.Content.ReadAsStringAsync();
                        using var detailDoc = JsonDocument.Parse(detailJson);
                        var detailRoot = detailDoc.RootElement;

                        if (detailRoot.TryGetProperty("description_raw", out var description))
                            game.Description = description.GetString();

                        if (detailRoot.TryGetProperty("background_image", out var screenshot))
                            game.ScreenshotUrl = screenshot.GetString();
                    }
                }
            }
        }

    }
}
