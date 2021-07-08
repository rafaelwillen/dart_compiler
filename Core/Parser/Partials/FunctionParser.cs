using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser.Partials
{
    public class FunctionParser : AbstractParser
    {
        private StatementsParser statementsParser;

        public override Symbol StartParsing(Symbol startSymbol)
        {
            symbol = startSymbol;
            functionDefinition();
            return symbol;
        }

        public Symbol ParseFunctionSignature(Symbol symbol)
        {
            this.symbol = symbol;
            functionSignature();
            return this.symbol;
        }

        public Symbol ParseFunctionBody(Symbol symbol)
        {
            this.symbol = symbol;
            functionBodyA();
            return this.symbol;
        }

        public Symbol ParseBlock(Symbol symbol)
        {
            this.symbol = symbol;
            block();
            return this.symbol;
        }

        public Symbol ParseFormalParameter(Symbol symbol)
        {
            this.symbol = symbol;
            formalParameterA();
            return this.symbol;
        }

        private void functionDefinition()
        {
            functionSignature();
            functionBodyA();
        }

        private void functionBodyA()
        {
            if (symbol.Token == Token.TokenKeywordAsync)
            {
                readNextSymbol();
            }

            functionBodyB();
        }

        private void functionBodyB()
        {
            if (symbol.Token == Token.TokenAssignment)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenGreater)
                {
                    readNextSymbol();
                    var expressionParser = new ExpressionParser();
                    symbol = expressionParser.StartParsing(symbol);
                    if (symbol.Token == Token.TokenEndStatement)
                    {
                        readNextSymbol();
                    }
                    else
                    {
                        error("Esperava um ';'");
                    }
                }
                else
                {
                    error("Esperava um '>'");
                }
            }
            else
            {
                block();
            }
        }

        private void block()
        {
            if (symbol.Token == Token.TokenOpenCBrackets)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava um '{'");
            }
            statementsParser = new StatementsParser();
            symbol = statementsParser.ParseStatements(symbol);
            if (symbol.Token == Token.TokenCloseCBrackets)
            {
                readNextSymbol();
                return;
            }
            error("Esperava um '}'");
        }

        private void functionSignature()
        {
            if (symbol.Token == Token.TokenKeywordVoid || symbol.isDataType())
            {
                readNextSymbol();
            }

            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
                formalParameterPart();
            }
            else
            {
                error("Esperava um identificador");
            }
        }

        private void formalParameterPart()
        {
            if (symbol.Token == Token.TokenOpenParenteses)
            {
                readNextSymbol();
                formalParameterA();
            }
            else
            {
                error("Esperava um '('");
            }
        }

        private void formalParameterA()
        {
            if (symbol.Token == Token.TokenCloseParenteses)
            {
                readNextSymbol();
                return;
            }
            normalParameters();
            if (symbol.Token == Token.TokenCloseParenteses)
            {
                readNextSymbol();
                return;
            }
            error("Esperava ')'");
        }

        private void normalParameters()
        {
            normalParameter();
            while (symbol.Token == Token.TokenComma)
            {
                readNextSymbol();
                normalParameter();
            }
        }

        private void normalParameter()
        {
            if (symbol.isDataType())
            {
                readNextSymbol();
            }

            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
                return;
            }
            error("Esperava um identifier");
        }
    }
}