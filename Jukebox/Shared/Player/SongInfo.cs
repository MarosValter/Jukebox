using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jukebox.Player.Base;

namespace Jukebox.Shared.Player
{
    public class SongInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }
        public string AddedBy { get; set; }
        public PlayerType Type { get; set; }
        public bool IsPlaying { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SongInfo info &&
                   Id == info.Id &&
                   Type == info.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type);
        }
    }

    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.FromTicks(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Ticks);
        }
    }
}
