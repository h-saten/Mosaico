using System.Text.RegularExpressions;

namespace KangaExchange.SDK
{
    public static class PaymentGatewayIdFromUrlException
    {
        internal static string Extract(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            var regExp = @"^(https:\/\/.*\/tpg_payment\/)([a-zA-Z0-9]*)$";
            var match = Regex.Match(url, regExp, RegexOptions.IgnoreCase);
            return match.Groups[2].Value;
        }   
    }
}