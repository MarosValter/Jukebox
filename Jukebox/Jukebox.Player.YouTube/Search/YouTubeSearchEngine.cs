using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Jukebox.Player.Search;

namespace Jukebox.Player.YouTube.Search
{
    public class YouTubeSearchEngine : ISearchEngine
    {
        public const string ClientName = "YouTubeHttpClient";
        private readonly HttpClient _httpClient;
        private const string APIKey = "AIzaSyAm-hFT1fhw92UEkZhUr5D5EiumjLRUE5I";

        public YouTubeSearchEngine(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(ClientName);
        }

        public async Task<IList<SearchResultInfo>> Search(string q, int maxResults = 5)
        {
            Console.WriteLine($"HttpClient base address: {_httpClient.BaseAddress}");
            Console.WriteLine($"HttpClient query: {GetQuery(q, maxResults)}");
            var result = await _httpClient.GetFromJsonAsync<YouTubeSearchResponse>(GetQuery(q, maxResults));

            return result?.Items.Select(MapSearchResult).ToList();
        }

        private static string GetQuery(string q, int maxResults)
        {
            if (maxResults < 0 || maxResults > 50)
            {
                maxResults = 5;
            }

            return $"youtube/v3/search?part=snippet&order=viewCount&maxResults={maxResults}&q={q}&type=video&videoDefinition=high&key={APIKey}";
        }

        private static SearchResultInfo MapSearchResult(YouTubeResponseItem item)
        {
            return new SearchResultInfo
            {
                Id = item.Id.VideoId,
                Title = item.Snippet.Title,
                ThumbnailUrl = item.Snippet.Thumbnails?["default"]?.Url,
                Type = PlayerType.YouTube
            };
        }
    }
}
