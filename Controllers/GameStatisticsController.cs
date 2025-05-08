using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroTrackRestNet.Data;
using RetroTrackRestNet.DTO;

namespace RetroTrackRestNet.Controllers
{
    [Route("/rest/api/[controller]")]
    [ApiController]
    public class GameStatisticsController : ControllerBase
    {
        private readonly DataContext _context;

        public GameStatisticsController(DataContext context)
        {
            _context = context;
        }

        // GET: /retrotrack/api/GameStatistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameStatisticsDto>>> GetStatistics()
        {
            var statistics = await _context.GameSessions
            .Join(_context.Games,
                  gs => gs.GameId,
                  g => g.Id,
                  (gs, g) => new { gs, g })
            .GroupBy(x => new { x.gs.GameId, x.g.Title })
            .Select(group => new GameStatisticsDto
            {
                GameId = group.Key.GameId,
                GameTitle = group.Key.Title,
                TotalSessions = group.Count(),
                TotalMinutesPlayed = group.Sum(x => x.gs.MinutesPlayed),
                AverageMinutesPerSession = group.Average(x => x.gs.MinutesPlayed),
                DistinctPlayers = group.Select(x => x.gs.PlayerId).Distinct().Count()
            })
            .ToListAsync();

            return Ok(statistics);
        }
    }
}
