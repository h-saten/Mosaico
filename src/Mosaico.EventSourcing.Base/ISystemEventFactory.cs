namespace Mosaico.EventSourcing.Base
{
    public interface ISystemEventFactory
    {
        SystemEvent Create<T>(string source, string type, T body) where T : class;
    }
}