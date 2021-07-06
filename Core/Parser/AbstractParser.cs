using System;
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
            Console.WriteLine($"Erro - encontrado caracter {symbol.Token} - {message} na linha {symbol.LineOfCode}");
        }

        public abstract Symbol StartParsing(Symbol startSymbol);
    }
}