using System.Collections.Generic;
using System.IO;

namespace FX.Test.Core
{
    public interface ITradeLoader
    {
        IEnumerable<ITrade> GetTrades(StreamReader stream);
    }
}