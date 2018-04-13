using System;

namespace FX.Test.Core
{
    public class EuropianTradeOption: Trade, ITradeOption
    {
        private readonly Func<ITrade, CalcOptionParameters, double> _getPut;
        private readonly Func<ITrade, CalcOptionParameters, double> _getCall;

        public EuropianTradeOption(ITrade trade,
            CalcOptionParameters parameters, 
            Func<ITrade, CalcOptionParameters, double> getPut,
            Func<ITrade, CalcOptionParameters, double> getCall)
        {
            Init(trade);
            _getPut = getPut;
            _getCall = getCall;
            Parameters = parameters;
        }

        public double Put { get { return _getPut(this, Parameters); }  }

        public double Call { get { return _getCall(this, Parameters); } }

        public CalcOptionParameters Parameters { get; }
    }
}