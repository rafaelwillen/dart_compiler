using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser.Partials
{
    public class StatementsParser : AbstractParser
    {
        FunctionParser functionParser;
        public override Symbol StartParsing(Symbol startSymbol)
        {
            symbol = startSymbol;
            statement();
            return symbol;
        }

        public Symbol ParseStatements(Symbol startSymbol)
        {
            symbol = startSymbol;
            statements();
            return symbol;
        }

        public Symbol ParseType(Symbol startSymbol)
        {
            symbol = startSymbol;
            type();
            return symbol;
        }

        private void statements()
        {
            while (symbol.Token != Token.TokenCloseCBrackets && symbol.Token != Token.TokenEndStatement
             && symbol.Token != Token.TokenKeywordCase && symbol.Token != Token.TokenKeywordDefault)
            {
                statement();
            }
        }

        private void statement()
        {
            // while (symbol.Token != Token.TokenColon)
            // {
            //     readNextSymbol();
            //     label();
            // }
            nonLabelledStatement();
        }

        private void nonLabelledStatement()
        {
            switch (symbol.Token)
            {
                case Token.TokenOpenCBrackets:
                    functionParser = new FunctionParser();
                    symbol = functionParser.ParseBlock(symbol);
                    break;
                case Token.TokenKeywordBreak:
                    breakStatement();
                    break;
                case Token.TokenKeywordContinue:
                    continueStatement();
                    break;
                case Token.TokenKeywordReturn:
                    returnStatement();
                    break;
                case Token.TokenKeywordFor:
                case Token.TokenKeywordAwait:
                    forStatement();
                    break;
                case Token.TokenKeywordWhile:
                    whileStatement();
                    break;
                case Token.TokenKeywordDo:
                    doStatement();
                    break;
                case Token.TokenKeywordSwitch:
                    switchStatement();
                    break;
                case Token.TokenKeywordIf:
                    ifStatement();
                    break;
                case Token.TokenKeywordTry:
                    // tryStatement();
                    break;
                case Token.TokenID:
                    localVarDeclaration();
                    break;
                // FIXME: Change check the first terminal o fhe expressionStatement, 
                // it's causing bugs
                default:
                    expressionStatement();
                    break;
            }
        }

        private void localVarDeclaration()
        {
            VariablesParser variableParser = new VariablesParser();
            symbol = variableParser.ParseInitializedVariableDeclaration(symbol);
            if (symbol.Token == Token.TokenEndStatement)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava um ';'");
            }
        }

        private void breakStatement()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenEndStatement)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava um ';'");
            }
        }

        private void continueStatement()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenEndStatement)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava um ';'");
            }
        }

        private void returnStatement()
        {
            readNextSymbol();
            // TODO: Parse expression optionally
            if (symbol.Token == Token.TokenEndStatement)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava um ';'");
            }
        }

        private void expressionStatement()
        {
            // TODO: Parse expression optionally
            if (symbol.Token == Token.TokenEndStatement)
            {
                readNextSymbol();
            }
            else
            {
                error("Esperava um ';'");
            }
        }

        #region For Loop
        private void forStatement()
        {
            if (symbol.Token == Token.TokenKeywordAwait)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenKeywordFor)
                {
                    readNextSymbol();
                }
                else
                    error("Esperava o 'for'");
            }
            else
            {
                if (symbol.Token == Token.TokenOpenParenteses)
                {
                    readNextSymbol();
                }
                else
                {
                    error("Esperava o '('");
                }

                forLoopParts();

                if (symbol.Token == Token.TokenCloseParenteses)
                {
                    readNextSymbol();
                }
                else
                {
                    error("Esperava o ')'");
                }

                statement();
            }
        }

        private void forLoopParts()
        {
            forInitStatement();
            // TODO: Parse expression optionally
            if (symbol.Token == Token.TokenEndStatement)
                readNextSymbol();
            else
                error("Ciclo 'for' incompleto");
            // TODO: Parse expression optionally

            // For each is not implemented
        }

        private void forInitStatement()
        {
            // TODO: Parse localVarDeclaration
            // TODO: Parse expression
            if (symbol.Token == Token.TokenEndStatement)
                readNextSymbol();
            else error("Esperava um ';'");
        }
        #endregion

        #region While
        private void whileStatement()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenOpenParenteses)
                readNextSymbol();
            else
                error("Esperava um '('");
            // TODO: Parse expression
            if (symbol.Token == Token.TokenCloseParenteses)
                readNextSymbol();
            else
                error("Esperava um ')'");
            statement();
        }
        #endregion

        #region Do While Loop
        private void doStatement()
        {
            readNextSymbol();
            statement();
            if (symbol.Token == Token.TokenKeywordWhile)
                readNextSymbol();
            else
                error("Esperava um 'while'");
            if (symbol.Token == Token.TokenOpenParenteses)
                readNextSymbol();
            else
                error("Esperava um '('");
            // TODO: Parse expression optionally
            if (symbol.Token == Token.TokenCloseParenteses)
                readNextSymbol();
            else
                error("Esperava um ')'");
            if (symbol.Token == Token.TokenEndStatement)
                readNextSymbol();
            else
                error("Esperava um ';'");
        }
        #endregion

        #region Switch
        private void switchStatement()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenOpenParenteses)
                readNextSymbol();
            else
                error("Esperava um '('");
            // TODO: Parse expression optionally
            if (symbol.Token == Token.TokenCloseParenteses)
                readNextSymbol();
            else
                error("Esperava um ')'");
            if (symbol.Token == Token.TokenOpenCBrackets)
                readNextSymbol();
            else error("Esperava um '{'");
            while (symbol.Token != Token.TokenKeywordDefault && symbol.Token != Token.TokenOpenCBrackets)
            {
                switchCase();
            }
            if (symbol.Token == Token.TokenKeywordDefault)
            {
                defaultCase();
                if (symbol.Token == Token.TokenCloseCBrackets)
                    readNextSymbol();
                else error("Esperava um '}'");
            }
        }

        private void switchCase()
        {
            if (symbol.Token == Token.TokenKeywordCase)
                readNextSymbol();
            else error("Esperava um 'case'");
            // TODO: Parse expression
            if (symbol.Token == Token.TokenColon)
                readNextSymbol();
            else error("Esperava um ':'");
            statements();
        }

        private void defaultCase()
        {
            if (symbol.Token == Token.TokenKeywordDefault)
                readNextSymbol();
            else error("Esperava um 'default'");
            if (symbol.Token == Token.TokenColon)
                readNextSymbol();
            else error("Esperava um ':'");
            statements();
        }
        #endregion

        #region If
        private void ifStatement()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenOpenParenteses)
                readNextSymbol();
            else
                error("Esperava um '('");
            // TODO: Parse expression
            if (symbol.Token == Token.TokenCloseParenteses)
                readNextSymbol();
            else
                error("Esperava um ')'");
            statement();
            if (symbol.Token == Token.TokenKeywordElse)
            {
                readNextSymbol();
                statement();
            }
        }
        #endregion

        #region Type
        private void type()
        {
            if (symbol.Token == Token.TokenID)
                readNextSymbol();
            else error("Esperava um tipo");
            if (symbol.Token == Token.TokenDot)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenID)
                    readNextSymbol();
                else error("Esperava um tipo");
            }
        }
        #endregion

        #region Static Declaration  
        private void staticFinalDeclarations()
        {
            staticFinalDeclaration();
            while (symbol.Token == Token.TokenComma)
            {
                readNextSymbol();
                staticFinalDeclaration();
            }
        }

        private void staticFinalDeclaration()
        {
            if (symbol.Token == Token.TokenID)
                readNextSymbol();
            else error("Esperava um identificador");
            if (symbol.Token == Token.TokenAssignment)
                readNextSymbol();
            else error("Esperava o operador de atribuição");
            // TODO: Parse expression
        }
        #endregion

    }
}