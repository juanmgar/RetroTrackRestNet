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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGameCollection>()
                .HasOne(ugc => ugc.Game)
                .WithMany(g => g.UserGameCollections)
                .HasForeignKey(ugc => ugc.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Game> Games { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<UserGameCollection> UserGameCollections { get; set; }

    }

}
