using System;
using System.Text;

namespace Jukebox.Client.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToDurationString(this TimeSpan duration, bool includeMillis = false)
        {
            var sb = new StringBuilder();
            if ((int)duration.TotalHours > 0)
            {
                sb.Append((int)duration.TotalHours);
                sb.Append(":");
            }

            sb.Append(duration.ToString(@"m\:ss"));

            if (includeMillis)
            {
                sb.Append(".");
                sb.Append(duration.ToString("FFF"));
            }

            return sb.ToString();
        }
    }
}
