using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser.Partials
{
    public class FunctionParser : AbstractParser
    {

        public override Symbol StartParsing(Symbol startSymbol)
        {
            symbol = startSymbol;
            functionDefinition();
            return symbol;
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
                    // TODO: Parse <expression>
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

            // TODO: Parse <statements>
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
            if (symbol.Token == Token.TokenOpenParenteses)
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