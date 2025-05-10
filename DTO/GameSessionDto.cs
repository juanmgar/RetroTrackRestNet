namespace RetroTrackRestNet.DTO
{
    public class GameSessionDto
    {
        public string PlayerId { get; set; }
        public int GameId { get; set; }
        public DateTime PlayedAt { get; set; }
        public int MinutesPlayed { get; set; }
        public IFormFile? Screenshot { get; set; }
    }

}
