using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser.Partials
{
    public class VariablesParser : AbstractParser
    {
        private StatementsParser statementsParser;

        public override Symbol StartParsing(Symbol startSymbol)
        {
            symbol = startSymbol;
            variableDeclaration();
            return symbol;
        }

        public Symbol ParseInitializedVariableDeclaration(Symbol startSymbol)
        {
            symbol = startSymbol;
            initializedVariableDeclaration();
            return symbol;
        }

        private void variableDeclaration()
        {
            declaredIdentifier();
            while (symbol.Token == Token.TokenComma)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenID)
                    readNextSymbol();
                else
                    error("Esperava um identificador");
            }
        }

        private void declaredIdentifier()
        {
            finalConstVarOrType();
            if (symbol.Token == Token.TokenID)
                readNextSymbol();
            else error("Esperava um identificador");
        }

        private void finalConstVarOrType()
        {
            if (symbol.Token == Token.TokenKeywordFinal || symbol.Token == Token.TokenKeywordConst)
            {
                readNextSymbol();
                var statementParser = new StatementsParser();
                statementParser.ParseType(symbol);
            }
            else if (symbol.Token == Token.TokenKeywordVar)
            {
                readNextSymbol();
            }
            else
            {
                statementsParser = new StatementsParser();
                symbol = statementsParser.ParseType(symbol);
            }
        }

        #region Initialized Variable Declaration
        private void initializedVariableDeclaration()
        {
            declaredIdentifier();
            if (symbol.Token == Token.TokenAssignment)
            {
                readNextSymbol();
                // TODO: Parse <expression>
            }

            while (symbol.Token == Token.TokenComma)
            {
                readNextSymbol();
                initializedIdentifier();
            }
        }

        private void initializedIdentifier()
        {
            if (symbol.Token == Token.TokenID)
                readNextSymbol();
            else
                error("Esperava um identificador");

            if (symbol.Token == Token.TokenAssignment)
            {
                readNextSymbol();
                // Parse <expression>
            }
        }
        #endregion
    }
}