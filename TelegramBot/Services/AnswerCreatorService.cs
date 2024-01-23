using System.Text;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    internal static class AnswerCreatorService
    {
        public static string Create(Currency currency)
        {
            var result = new StringBuilder();

            result.Append($"{currency.ReqVal} {currency.ReqValName} = {currency.ConVal} {currency.ConValName}\n");

            if (currency is not { ReqVal: 1 })
            {
                result.Append($"1 {currency.ReqValName} = {currency.ReqValUnit} {currency.ConValName}\n");
            }

            result.Append($"1 {currency.ConValName} = {currency.ConValUnit} {currency.ReqValName}\n");
            result.Append($"Актуально на {currency.Date}");

            return result.ToString();
        }
    }
}