using Microsoft.EntityFrameworkCore;
using RetroTrackRestNet.Model;

namespace RetroTrackRestNet.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=gamestats.db");
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<UserGameCollection> UserGameCollections { get; set; }

    }

}
