using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
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
            var result = await _httpClient.GetFromJsonAsync<YouTubeSearchResponse>(GetSearchQuery(q, maxResults));
            var detailsQuery = GetDetailsQuery(result.Items.Select(x => x.Id.VideoId).ToArray());
            var details = await _httpClient.GetFromJsonAsync<YouTubeDetailsResponse>(detailsQuery);

            return details?.Items.Select(MapSearchResult).ToList();
        }

        private static string GetSearchQuery(string q, int maxResults)
        {
            if (maxResults < 0 || maxResults > 50)
            {
                maxResults = 5;
            }

            return $"search?order=viewCount&maxResults={maxResults}&q={q}&type=video&videoDefinition=high&key={APIKey}";
        }

        private static string GetDetailsQuery(string[] ids)
        {
            string[] parts =
            {
                "snippet",
                "contentDetails",
                "statistics"
            };
            return $"videos?part={HttpUtility.UrlEncode(string.Join(",", parts))}&id={HttpUtility.UrlEncode(string.Join(",", ids))}&key={APIKey}";
        }

        private static SearchResultInfo MapSearchResult(YouTubeDetailsResponseItem item)
        {
            return new SearchResultInfo
            {
                Id = item.Id,
                Title = item.Snippet?.Title,
                ViewCount = item.Statistics?.ViewCount ?? 0,
                Duration = item.ContentDetails?.Duration ?? TimeSpan.Zero,
                ThumbnailUrl = item.Snippet?.Thumbnails?["default"]?.Url,
                Type = PlayerType.YouTube
            };
        }
    }
}
