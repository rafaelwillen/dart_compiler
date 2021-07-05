using System;
using dart_compiler.Core.Utils;
using dart_compiler.Core.Parser.Partials;

namespace dart_compiler.Core.Parser
{
    public class Parser : AbstractParser
    {
        private FunctionParser functionParser;
        public Parser()
        {
            functionParser = new FunctionParser();
            readNextSymbol();
        }

        public override Symbol StartParsing(Symbol startSymbol)
        {
            topLevelDefinition();
            return symbol;
        }

        private void topLevelDefinition()
        {
            if (symbol.Token == Token.TokenKeywordVoid || symbol.Token == Token.TokenID || symbol.isDataType())
            {
                symbol = functionParser.StartParsing(symbol);
            }
            else if (symbol.Token == Token.TokenKeywordImport)
            {
                libraryImport();
            }
        }

        private void libraryImport()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenString)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava uma URL ou string");
            }

            if (symbol.Token == Token.TokenEndStatement)
            {
                readNextSymbol();
                return;
            }
            error("Esperava ';'");
        }
    }
}