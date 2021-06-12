
using System;
using dart_compiler.Core;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser
{
    public class Parser
    {
        private Symbol symbol;

        public Parser()
        {
            symbol = TableSymbol.GetNextSymbol();
        }

        public void Parse()
        {
            variableDeclaration();
        }

        private void readNextSymbol()
        {
            symbol = TableSymbol.GetNextSymbol();
        }

        private void error(string message)
        {
            Console.WriteLine($"Erro - encontrado caracter {symbol.Token} - {message} at line {symbol.LineOfCode}");
            Console.ReadLine();
        }

        #region Var Declaration
        /// <summary>
        /// varDeclaration ::= declaredIdentifier (',' IDENTIFIER)*
        /// </summary>
        private void variableDeclaration()
        {
            declaredIdentifier();
            if (symbol.Token == Token.TokenEndStatement)
            {
                Console.WriteLine("Success");
                return;
            }
            else
            {
                error("Expected ';'");
            }
            bool loopFalse = true;
            while (loopFalse)
            {
                loopFalse = symbol.Token == Token.TokenComma;
                if (loopFalse) readNextSymbol();
                else error("Expected ','");
                loopFalse = symbol.Token == Token.TokenID;
                if (loopFalse) readNextSymbol();
                else error("Expected an identifier");
                loopFalse = symbol.Token == Token.TokenComma;
            }
            Console.WriteLine("Success");
        }



        /// <summary>
        /// declaredIdentifier ::= finalConstVarOrType IDENTIFIER
        /// </summary>
        private void declaredIdentifier()
        {
            finalConstVarOrType();
            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
            }
            else
            {
                error("Expected an identifier");
            }
        }

        /// <summary>
        /// finalConstVarOrType ::=  ('final' | 'const') type? | ('var' | type)
        /// </summary>
        private void finalConstVarOrType()
        {
            if (symbol.Token == Token.TokenKeywordFinal || symbol.Token == Token.TokenKeywordConst)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenID || symbol.isDataType())
                {
                    type();
                }
                else
                {
                    return;
                }
            }
            else if (symbol.Token == Token.TokenKeywordVar)
            {
                readNextSymbol();
            }
            else
            {
                type();
            }
        }

        /// <summary>
        /// type ::= (IDENTIFIER | DATA_TYPE) typeArguments?
        /// </summary>
        private void type()
        {
            if (symbol.Token == Token.TokenID || symbol.isDataType())
            {
                readNextSymbol();
            }
            else
            {
                error("Expected a data type");
            }

            if (symbol.Token == Token.TokenLess)
            {
                typeArguments();
            }
        }

        /// <summary>
        /// typeArguments ::= '<' type (',' type)* '>'
        /// </summary>
        private void typeArguments()
        {
            if (symbol.Token == Token.TokenLess)
            {
                readNextSymbol();
            }
            else
            {
                error("Expected a '<'");
            }

            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
            }
            else
            {
                error("Expected a identifier");
            }
            bool loopFalse = symbol.Token == Token.TokenComma;
            while (loopFalse)
            {
                loopFalse = symbol.Token == Token.TokenComma;
                if (loopFalse) readNextSymbol();
                else error("Expected ','");
                loopFalse = symbol.Token == Token.TokenID;
                if (loopFalse) readNextSymbol();
                else error("Expected an identifier");
                loopFalse = symbol.Token == Token.TokenComma;
            }

            if (symbol.Token == Token.TokenGreater)
            {
                readNextSymbol();
            }
            else
            {
                error("Expected a '>'");
            }

        }
        #endregion

    }
}