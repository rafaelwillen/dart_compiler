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
            Console.WriteLine($"Erro - encontrado caracter {symbol.Token} - {message} at line {symbol.LineOfCode}");
            Console.ReadLine();
        }

        public abstract Symbol StartParsing(Symbol startSymbol);
    }
}