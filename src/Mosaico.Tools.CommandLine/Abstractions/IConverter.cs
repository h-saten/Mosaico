using System.Threading.Tasks;

namespace Mosaico.Tools.CommandLine.Abstractions
{
    public interface IConverter<TSource, TDestination>
    {
        Task<TDestination> ConvertAsync(TSource source);
    }
}