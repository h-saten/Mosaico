namespace Mosaico.Base.Abstractions
{
    public interface IStringHasher
    {
        string CreateHash(string data);
        bool IsHashValid(string data, string hash);
    }
}