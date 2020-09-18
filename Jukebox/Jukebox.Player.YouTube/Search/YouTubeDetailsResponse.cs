using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jukebox.Player.YouTube.Search
{
    public class YouTubeDetailsResponse
    {
        public YouTubeDetailsResponseItem[] Items { get; set; }
    }

    public class YouTubeDetailsResponseItem
    {
        public string Id { get; set; }
        public YouTubeItemSnippet Snippet { get; set; }
        public YouTubeItemContentDetails ContentDetails { get; set; }
        public YouTubeItemStatistics Statistics { get; set; }
    }

    public class YouTubeItemSnippet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Dictionary<string, YouTubeItemThumbnail> Thumbnails { get; set; }
    }

    public class YouTubeItemContentDetails
    {
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }
    }

    public class YouTubeItemStatistics
    {
        [JsonConverter(typeof(IntegerConverter))]
        public int ViewCount { get; set; }
    }

    public class YouTubeItemThumbnail
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class IntegerConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return int.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            var result = TimeSpan.Zero;
            if (value[0] != 'P')
            {
                throw new FormatException();
            }

            if (!value.Contains("T"))
            {
                throw new FormatException();
            }

            var date = value.Skip(1).TakeWhile(c => c != 'T').ToArray();
            if (date.Length > 0)
            {
                if (date.Contains('Y'))
                {
                    var yearsStr = date.TakeWhile(c => c != 'Y').ToArray();
                    if (int.TryParse(new string(yearsStr), out var years))
                    {
                        result += TimeSpan.FromDays(365 * years);
                    }
                    date = date.Skip(yearsStr.Length + 1).ToArray();
                }

                if (date.Contains('M'))
                {
                    var monthsStr = date.TakeWhile(c => c != 'M').ToArray();
                    if (int.TryParse(new string(monthsStr), out var months))
                    {
                        result += TimeSpan.FromDays(30 * months);
                    }
                    date = date.Skip(monthsStr.Length + 1).ToArray();
                }

                if (date.Contains('D'))
                {
                    var daysStr = date.TakeWhile(c => c != 'D').ToArray();
                    if (int.TryParse(new string(daysStr), out var days))
                    {
                        result += TimeSpan.FromDays(days);
                    }
                    //date = date.Skip(daysStr.Length + 1).ToArray();
                }
            }

            var time = value.SkipWhile(c => c != 'T').Skip(1).ToArray();
            if (time.Contains('H'))
            {
                var hoursStr = time.TakeWhile(c => c != 'H').ToArray();
                if (int.TryParse(new string(hoursStr), out var hours))
                {
                    result += TimeSpan.FromHours(hours);
                }
                time = time.Skip(hoursStr.Length + 1).ToArray();
            }

            if (time.Contains('M'))
            {
                var minutesStr = time.TakeWhile(c => c != 'M').ToArray();
                if (int.TryParse(new string(minutesStr), out var minutes))
                {
                    result += TimeSpan.FromMinutes(minutes);
                }
                time = time.Skip(minutesStr.Length + 1).ToArray();
            }

            if (time.Contains('S'))
            {
                var secondsStr = time.TakeWhile(c => c != 'S').ToArray();
                if (int.TryParse(new string(secondsStr), out var seconds))
                {
                    result += TimeSpan.FromSeconds(seconds);
                }
                //date = date.Skip(daysStr.Length + 1).ToArray();
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
