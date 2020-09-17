using System;
using Jukebox.Player.YouTube.Search;
using Microsoft.Extensions.DependencyInjection;

namespace Jukebox.Player.YouTube.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddYouTube(this IServiceCollection services)
        {
            // register search engine http client
            services.AddHttpClient(YouTubeSearchEngine.ClientName, (client) =>
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/");
            });

            return services;
        }
    }
}
