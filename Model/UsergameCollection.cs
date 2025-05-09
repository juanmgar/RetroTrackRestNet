namespace RetroTrackRestNet.Model
{
    public class UserGameCollection
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string User { get; set; }
        public Game Game { get; set; }


    }

}
