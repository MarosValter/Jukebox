using System;
using System.Net.Http;
using Jukebox.Player;
using Jukebox.Player.Search;
using Jukebox.Player.YouTube.Search;

namespace Jukebox.Client.Search
{
    public class SearchEngineProvider : ISearchEngineProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SearchEngineProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public ISearchEngine GetSearchEngine(PlayerType type)
        {
            return type switch
            {
                PlayerType.YouTube => new YouTubeSearchEngine(_httpClientFactory),
                PlayerType.Spotify => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}