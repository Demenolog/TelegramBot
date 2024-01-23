using HtmlAgilityPack;
using TelegramBot.Models;
using TelegramBot.Services;

namespace TelegramBot.Infrastructure.Web
{
    internal class WebParser
    {
        public Currency GetData(string request)
        {
            var url = CreateUrl(request);

            var page = LoadPage(url);

            var parsedData = ParseData(page);

            var result = GetResult(parsedData);

            return result;
        }

        private string CreateUrl(string request)
        {
            var info = request.Trim()
                .Split(' ');

            var amount = info[0];
            var from = info[1].ToUpper();
            var to = from switch
            {
                "USD" => "RUB",
                "RUB" => "USD",
                _ => throw new ArgumentException()
            };

            return $"https://www.xe.com/currencyconverter/convert/?Amount={amount}&From={from}&To={to}";
        }

        private HtmlDocument LoadPage(string url)
        {
            var web = new HtmlWeb();

            HtmlDocument page = web.Load(url);

            return page;
        }

        private List<string> ParseData(HtmlDocument document)
        {
            var htmlElements = document.DocumentNode.QuerySelector("main")
                .QuerySelectorAll("p");

            var requestedValues = htmlElements.Take(2)
                .Select(x => x.InnerText
                    .Split(' ', 2))
                .SelectMany(x => x);

            var unitValues = htmlElements.Skip(2)
                .SelectMany(x => x.InnerText
                    .Split('=')
                    .SelectMany(y => y.Trim().Split(' ')));

            var lastUpdateDate = document.DocumentNode.QuerySelector("main")
                .QuerySelector("div.jcIWiH")
                .InnerText
                .Split('—')[1]
                .Trim();

            var result = new List<string>();
            result.AddRange(requestedValues);
            result.AddRange(unitValues);
            result.Add(lastUpdateDate);

            return result;
        }

        private Currency GetResult(List<string> data)
        {
            var currency = new Currency();

            currency.ReqVal = StringConvertorService.ToDouble(data[0]);
            currency.ConVal = StringConvertorService.ToDouble(data[2]);
            currency.Date = StringConvertorService.ToDateTime(data.Last());

            if (Math.Abs(StringConvertorService.ToDouble(data[0]) - 1) != 0)
            {
                currency.ReqValUnit = StringConvertorService.ToDouble(data[6]);
                currency.ReqValName = data[5];
                currency.ConValUnit = StringConvertorService.ToDouble(data[10]);
                currency.ConValName = data[7];
            }
            else
            {
                currency.ReqValUnit = StringConvertorService.ToDouble(data[2]);
                currency.ReqValName = data[7];
                currency.ConValUnit = StringConvertorService.ToDouble(data[6]);
                currency.ConValName = data[5];
            }

            return currency;
        }
    }
}