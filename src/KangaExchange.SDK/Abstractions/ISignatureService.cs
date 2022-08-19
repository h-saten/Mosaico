namespace KangaExchange.SDK.Abstractions
{
    public interface ISignatureService
    {
        string GenerateSignature(object requestBody, string secretKey);
    }
}