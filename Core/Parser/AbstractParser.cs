using System;
using dart_compiler.Core.ErrorReport;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser
{
    public abstract class AbstractParser
    {
        protected Symbol symbol;
        protected void readNextSymbol()
        {
            symbol = TableSymbol.GetNextSymbol();
        }

        protected void error(string message)
        {
            var exception = new Exception($"{message} na {symbol.LineOfCode}");
            ErrorList.AddError(exception);
        }

        public abstract Symbol StartParsing(Symbol startSymbol);
    }
}