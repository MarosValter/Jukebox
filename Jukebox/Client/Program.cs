using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jukebox.Client.Manager;
using Jukebox.Client.Search;
using Jukebox.Player.Manager;
using Jukebox.Player.Search;
using Jukebox.Player.YouTube.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Jukebox.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            ConfigureServices(builder.Services, builder.HostEnvironment);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment environment)
        {
            services.AddSingleton(new HttpClient { BaseAddress = new Uri(environment.BaseAddress) });

            services.AddSingleton<ISearchEngineProvider, SearchEngineProvider>();
            services.AddSingleton<IPlayerManager, PlayerManager>();
            services.AddYouTube();
        }
    }
}
