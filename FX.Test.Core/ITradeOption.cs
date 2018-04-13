
namespace FX.Test.Core
{
    public interface ITradeOption:ITrade
    {
        double Put { get; }
        double Call { get; }
    }
}