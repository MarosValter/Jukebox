using Jukebox.Shared.Player;
using System.Text.Json;

namespace Jukebox.Shared.Serializer
{
    public static class SerializerOptions
    {
        public static void ConfigureOptions(JsonSerializerOptions options)
        {
            options.Converters.Add(new TimeSpanConverter());
        }
    }
}
