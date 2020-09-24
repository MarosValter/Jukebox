using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Extensions.Logging;
using Fluxor;
using Jukebox.Client.HubStore;
using Jukebox.Client.Manager;
using Jukebox.Client.Search;
using Jukebox.Player.Manager;
using Jukebox.Player.Search;
using Jukebox.Player.YouTube.Extensions;
using Jukebox.Shared.Store.States;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddLogging(options =>
            {
                options.AddBrowserConsole();
                options.SetMinimumLevel(LogLevel.Information);
            });

            services.AddSingleton(new HttpClient { BaseAddress = new Uri(environment.BaseAddress) });

            // Add Fluxor
            services.AddFluxor(options =>
            {
                options.ScanAssemblies(typeof(RoomState).Assembly);
                //options.UseReduxDevTools();
            });

            services.AddSingleton<ISearchEngineProvider, SearchEngineProvider>();
            services.AddSingleton<IPlayerManager, PlayerManager>();
            services.AddSingleton<IHubStore, HubStore.HubStore>();
            services.AddYouTube();
        }
    }
}
