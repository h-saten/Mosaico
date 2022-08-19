namespace Mosaico.Events.Base
{
    public interface IEventFactory
    {
        CloudEvent CreateEvent<TPayload>(string path, TPayload payload) where TPayload : class;
    }
}