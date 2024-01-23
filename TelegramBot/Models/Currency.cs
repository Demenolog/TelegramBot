using System.Text;

namespace TelegramBot.Models
{
    internal class Currency
    {
        public double? ReqVal { get; set; }
        public double? ReqValUnit { get; set; }
        public string? ReqValName { get; set; }

        public double? ConVal { get; set; }
        public double? ConValUnit { get; set; }
        public string? ConValName { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"{ReqVal} {ReqValName} = {ConVal} {ConValName}";
        }
    }
}