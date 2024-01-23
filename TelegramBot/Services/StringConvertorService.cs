using System.Globalization;

namespace TelegramBot.Services
{
    internal static class StringConvertorService
    {
        public static double ToDouble(string text)
        {
            return double.Parse(text.Replace(',', ' ')
                .Replace('.', ','));
        }

        public static DateTime ToDateTime(string text)
        {
            string[] array = text.Split(' ')
                .Skip(2)
                .Select(x => x.TrimEnd(','))
                .ToArray();

            string dateString = $"{array[0]} {array[1]} {array[2]} {array[3]} {array[4]}";

            return DateTime.ParseExact(dateString, "MMM d yyyy HH:mm UTC", CultureInfo.InvariantCulture).ToLocalTime();
        }
    }
}