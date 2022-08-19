namespace Mosaico.Base.Abstractions
{
    public interface IStringGenerator
    {
        public string Generate(long size = 32);
    }
}