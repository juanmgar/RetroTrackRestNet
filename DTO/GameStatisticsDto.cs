namespace RetroTrackRestNet.DTO
{
    public class GameStatisticsDto
    {
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public int TotalSessions { get; set; }
        public int TotalMinutesPlayed { get; set; }
        public double AverageMinutesPerSession { get; set; }
        public int DistinctPlayers { get; set; }
    }
}
