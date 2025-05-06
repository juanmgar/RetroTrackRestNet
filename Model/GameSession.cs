namespace RetroTrackRestNet.Model
{
    public class GameSession
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public String PlayerId { get; set; }
        public DateTime PlayedAt { get; set; }
        public int MinutesPlayed { get; set; }

    }
}
