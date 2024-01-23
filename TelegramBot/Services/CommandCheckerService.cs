namespace TelegramBot.Services
{
    internal static class CommandCheckerService
    {
        public static bool IsConversionCommand(string text)
        {
            if (text.Length >= 5)
            {
                var ending = text
                    .ToUpper()
                    .Trim()
                    .Substring(text.Length - 3);

                return ending is "USD" or "RUB";
            }
            else
            {
                return false;
            }
        }

        public static bool IsHelpCommand(string text) => text.ToUpper().Trim() == "HELP";
    }
}