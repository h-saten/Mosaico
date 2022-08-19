namespace Mosaico.Blockchain.Base.DAL.Events
{
    // Marker interface
    public interface IContractEvent
    {
        public static string Topic { get; }
        public static string ABI { get; }
    }
}