
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
            functionSignature();
            Console.WriteLine("Sucesso");
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

        #region Functions
        /// <summary>
        /// functionSignature ::= returnType? IDENTIFIER formalParameterPart
        /// </summary>
        private void functionSignature()
        {
            // TODO: Fix the optional return type for the function
            if (symbol.Token == Token.TokenKeywordVoid || symbol.Token == Token.TokenID || symbol.isDataType())
            {
                returnType();
            }

            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
            }
            else
            {
                error("Expected an identifier");
            }
            formalParameterPart();
            functionBodyA();
        }

        /// <summary>
        /// returnType ::= 'void' | type
        /// </summary>
        private void returnType()
        {
            if (symbol.Token == Token.TokenKeywordVoid)
                readNextSymbol();
            else
                type();
        }

        /// <summary>
        /// formalParameterPart ::= typeParameters? formalParameterList
        /// </summary>
        private void formalParameterPart()
        {
            if (symbol.Token == Token.TokenLess)
            {
                typeParameters();
            }

            formalParameterListA();
        }

        /// <summary>
        /// '<' typeParameter (',' typeParameter)* '>'
        /// </summary>
        private void typeParameters()
        {
            if (symbol.Token == Token.TokenLess)
            {
                readNextSymbol();
            }
            else
            {
                error("Expected an '<'");
            }

            typeParameter();
            bool isValid = true;
            while (isValid)
            {
                isValid = symbol.Token == Token.TokenComma;
                if (isValid) readNextSymbol();
                else error("Expected a ','");
                typeParameter();
                isValid = symbol.Token == Token.TokenComma;
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

        /// <summary>
        /// '(' formalParameterListB
        /// </summary>
        private void formalParameterListA()
        {
            if (symbol.Token == Token.TokenOpenParenteses)
            {
                readNextSymbol();
                formalParameterListB();
            }
            else
            {
                error("Expected a '('");
            }
        }

        /// <summary>
        /// ')' | normalParameters ','? ')'
        /// </summary>
        private void formalParameterListB()
        {
            if (symbol.Token == Token.TokenCloseParenteses)
            {
                readNextSymbol();
            }
            else
            {
                normalParameters();
                if (symbol.Token == Token.TokenComma)
                {
                    readNextSymbol();
                }

                if (symbol.Token == Token.TokenCloseParenteses)
                {
                    readNextSymbol();
                }
                else
                {
                    error("Expected a ')'");
                }
            }
        }

        /// <summary>
        /// normalParemeter (',' normalParemeter)*
        /// </summary>
        private void normalParameters()
        {
            normalParameter();
            bool isValid = symbol.Token == Token.TokenComma;
            while (isValid)
            {
                isValid = symbol.Token == Token.TokenComma;
                if (isValid) readNextSymbol();
                else error("Expected ','");
                normalParameter();
                isValid = symbol.Token == Token.TokenComma;
            }
        }

        private void normalParameter()
        {
            if (symbol.Token == Token.TokenID || symbol.isDataType())
            {
                type();
            }

            if (symbol.Token == Token.TokenID) readNextSymbol();
            else error("Expected a identifier");
        }

        /// <summary>
        /// (IDENTIFIER ('extends' type)?) | type
        /// </summary>
        private void typeParameter()
        {
            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenKeywordExtends)
                {
                    readNextSymbol();
                    type();
                }
            }
            else
            {
                type();
            }
        }

        /// <summary>
        /// 'async'? functionBodyB
        /// </summary>
        private void functionBodyA()
        {
            // TODO: Check string checking for Token 
            if (symbol.Lexeme == "async")
            {
                readNextSymbol();
            }
            functionBodyB();
        }

        /// <summary>
        /// '=>' expression ';' | block
        /// </summary>
        private void functionBodyB()
        {
            if (symbol.Token == Token.TokenAssignment)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenGreater)
                {
                    readNextSymbol();
                }
                else error("Expected a '>'");

                // TODO: Call expression()
                readNextSymbol();
                if (symbol.Token == Token.TokenEndStatement)
                {
                    readNextSymbol();
                }
                else
                {
                    error("Expected a ';'");
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
            else error("Expected '{'");

            // TODO: Call statements()

            if (symbol.Token == Token.TokenCloseCBrackets)
            {
                readNextSymbol();
            }
            else error("Expected '}'");
        }
        #endregion
    }
}