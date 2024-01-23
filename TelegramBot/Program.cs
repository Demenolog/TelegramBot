using System.Threading.Channels;
using HapCss;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Infrastructure.Web;
using TelegramBot.Services;

namespace TelegramBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var client = new TelegramBotClient("YOUR-APIKEY");

            client.StartReceiving(Update, Error);

            Console.WriteLine("[Нажмите любую клавишу, чтобы отключить бота]");

            Console.ReadKey();
        }

        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message ?? update.EditedMessage;

            if (message != null)
            {
                Console.WriteLine($"{message.Chat.Username} | {message.Date.AddHours(5).ToShortTimeString()} | {message.Text}");

                if (CommandCheckerService.IsConversionCommand(message.Text))
                {
                    var parser = new WebParser();

                    var result = parser.GetData(message.Text);

                    var answer = AnswerCreatorService.Create(result);

                    await botClient.SendTextMessageAsync(message.Chat.Id, answer, cancellationToken: token);
                }
                else if (CommandCheckerService.IsHelpCommand(message.Text))
                {
                    var text = "Доступные команды\n" +
                               "Конвертация валюты:\n" +
                               "[ X или X.XX ] [ USD или RUB ]\n" +
                               "100 usd\n" +
                               "125.5 usd\n" +
                               "100.95 rub";

                    await botClient.SendTextMessageAsync(message.Chat.Id, text, cancellationToken: token);
                }
                else
                {
                    var text = "Неизвестная команда !\n" +
                               "Список команд можно посмотреть набрав 'help'";

                    await botClient.SendTextMessageAsync(message.Chat.Id, text, cancellationToken: token);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Пустая строка !", cancellationToken: token);
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine($"[ERROR]: {arg2.Message}");

            throw new ApplicationException();
        }
    }
}