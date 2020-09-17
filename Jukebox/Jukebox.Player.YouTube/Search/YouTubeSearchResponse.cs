using System.Collections.Generic;

namespace Jukebox.Player.YouTube.Search
{
    public class YouTubeSearchResponse
    {
        public YouTubeResponseItem[] Items { get; set; }
    }

    public class YouTubeResponseItem
    {
        public YouTubeItemId Id { get; set; }
        public YouTubeItemSnippet Snippet { get; set; }
    }

    public class YouTubeItemId
    {
        public string VideoId { get; set; }
    }

    public class YouTubeItemSnippet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Dictionary<string, YouTubeItemThumbnail> Thumbnails { get; set; }
    }

    public class YouTubeItemThumbnail
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
