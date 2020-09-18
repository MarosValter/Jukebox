namespace Jukebox.Player.YouTube.Search
{
    public class YouTubeSearchResponse
    {
        public YouTubeSearchResponseItem[] Items { get; set; }
    }

    public class YouTubeSearchResponseItem
    {
        public YouTubeItemId Id { get; set; }
    }

    public class YouTubeItemId
    {
        public string VideoId { get; set; }
    }
}
