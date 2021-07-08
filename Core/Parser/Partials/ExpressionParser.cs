using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser.Partials
{
    public class ExpressionParser : AbstractParser
    {
        public override Symbol StartParsing(Symbol startSymbol)
        {
            symbol = startSymbol;
            expression();
            return symbol;
        }

        public Symbol ParseExpressions(Symbol startSymbol)
        {
            symbol = startSymbol;
            expressions();
            return symbol;
        }

        private void expression()
        {
            if (symbol.Token == Token.TokenKeywordThrow)
                throwExpression();
            else
            {
                switch (symbol.Token)
                {
                    case Token.TokenSubtraction:
                    case Token.TokenNot:
                    case Token.TokenKeywordAwait:
                    case Token.TokenKeywordThis:
                    case Token.TokenKeywordNew:
                    case Token.TokenOpenParenteses:
                        conditionalExpression();
                        break;
                    default:
                        if (isLiteral(symbol.Token))
                            conditionalExpression();
                        else
                        {
                            assignableExpression();
                            if (symbol.Token == Token.TokenEndStatement)
                                return;
                            if (isAssignmentOperator(symbol.Token))
                                assignmentOperator();
                            expression();
                        }
                        break;
                }
            }
        }

        private void expressions()
        {
            expression();
            while (symbol.Token == Token.TokenComma)
            {
                readNextSymbol();
                expression();
            }
        }

        private void assignableExpression()
        {
            if (symbol.Token == Token.TokenID)
            {
                readNextSymbol();
            }
            else
            {
                primary();
                assignableSelector();
                // while (symbol.Token == Token.TokenOpenBrackets)
                // {
                //     assignableSelector();
                // }
            }
        }

        private void conditionalExpression()
        {
            logicalOrExpression();

            if (symbol.Token == Token.TokenInterrogation)
            {
                readNextSymbol();
                expression();
                if (symbol.Token == Token.TokenColon)
                    readNextSymbol();
                else
                    error("Operador condicional inválido, esperava ':'");
                expression();
            }
        }

        private void throwExpression()
        {
            readNextSymbol();
            expression();
        }

        private void primary()
        {
            if (symbol.Token == Token.TokenKeywordThis)
            {
                readNextSymbol();
                return;
            }
            else if (isLiteral(symbol.Token))
                literal();
            else if (symbol.Token == Token.TokenKeywordNew)
            {
                newExpression();
            }
            else if (symbol.Token == Token.TokenOpenParenteses)
            {
                readNextSymbol();
                expression();
                if (symbol.Token == Token.TokenCloseParenteses)
                    readNextSymbol();
                else error("Esperava um ')'");
            }
            else
                functionExpression();
        }

        private void assignableSelector()
        {
            if (symbol.Token == Token.TokenOpenBrackets)
            {
                readNextSymbol();
                expression();
                if (symbol.Token == Token.TokenCloseBrackets)
                    readNextSymbol();
                else error("Esperava um ']'");
            }
            else if (symbol.Token == Token.TokenDot)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenID)
                    readNextSymbol();
                else error("Esperava um identificador");
            }
            else
                error("Expressão inválida");
        }

        private void functionExpression()
        {
            var functionParser = new FunctionParser();
            symbol = functionParser.ParseFormalParameter(symbol);
            symbol = functionParser.ParseFunctionBody(symbol);
        }

        private void newExpression()
        {
            readNextSymbol();
            var statementParser = new StatementsParser();
            symbol = statementParser.ParseType(symbol);
            if (symbol.Token == Token.TokenDot)
            {
                readNextSymbol();
                if (symbol.Token == Token.TokenID)
                    readNextSymbol();
                else error("Esperava um identificador");
            }
            arguments();
        }

        private void arguments()
        {
            if (symbol.Token == Token.TokenOpenParenteses)
                readNextSymbol();
            else
            {
                error("Esperava um '('");
                return;
            }
            if (symbol.Token == Token.TokenCloseParenteses)
            {
                readNextSymbol();
                return;
            }
            var expressionParser = new ExpressionParser();
            symbol = expressionParser.ParseExpressions(symbol);
            if (symbol.Token == Token.TokenCloseParenteses)
                readNextSymbol();
            else error("Esperava um ')'");
        }

        #region Operators

        private bool isAssignmentOperator(Token token)
        {
            switch (token)
            {
                case Token.TokenAssignment:
                case Token.TokenCA_AND:
                case Token.TokenCA_LShift:
                case Token.TokenCA_NOT:
                case Token.TokenCA_OR:
                case Token.TokenCA_RShift:
                case Token.TokenCAAdition:
                case Token.TokenCADivision:
                case Token.TokenCAModulus:
                case Token.TokenCAMultiplication:
                case Token.TokenCASubtraction:
                    return true;
                default: return false;
            }
        }

        private bool isEqualityOperator(Token token) => symbol.Token == Token.TokenEqual || symbol.Token == Token.TokenNotEqual;


        private bool isRelationalOperator(Token token) => symbol.Token == Token.TokenGreater || symbol.Token == Token.TokenGreaterEqual || symbol.Token == Token.TokenLess || symbol.Token == Token.TokenEqual;

        private void assignmentOperator()
        {
            if (isAssignmentOperator(symbol.Token))
                readNextSymbol();
            else
                error("Esperava um operador");
        }

        private void equalityOperator()
        {
            if (symbol.Token == Token.TokenEqual || symbol.Token == Token.TokenNotEqual)
                readNextSymbol();
            else
                error("Esperava um operador");
        }

        private void relationalOperator()
        {
            switch (symbol.Token)
            {
                case Token.TokenGreaterEqual:
                case Token.TokenGreater:
                case Token.TokenLessEqual:
                case Token.TokenLess:
                    readNextSymbol();
                    break;
                default:
                    error("Esperava um operador");
                    break;
            }
        }

        private void bitwiseOperator()
        {
            switch (symbol.Token)
            {
                case Token.TokenBitwiseAnd:
                case Token.TokenBitwiseNot:
                case Token.TokenBitwiseOR:
                    readNextSymbol();
                    break;
                default:
                    error("Esperava um operador");
                    break;
            }
        }

        private void shiftOperator()
        {
            if (symbol.Token == Token.TokenLeftShift || symbol.Token == Token.TokenRightShift)
                readNextSymbol();
            else
                error("Esperava um operador");
        }

        private void additiveOperator()
        {
            if (symbol.Token == Token.TokenAdition || symbol.Token == Token.TokenSubtraction)
                readNextSymbol();
            else
                error("Esperava um operador");
        }

        private void multiplicativeOperator()
        {
            switch (symbol.Token)
            {
                case Token.TokenMultiplication:
                case Token.TokenDivision:
                case Token.TokenModulus:
                    readNextSymbol();
                    break;
                default:
                    error("Esperava um operador");
                    break;
            }
        }

        private void isOperator()
        {
            if (symbol.Token == Token.TokenKeywordIs)
                readNextSymbol();
            else
                error("Esperava um operador");

            if (symbol.Token == Token.TokenNot)
                readNextSymbol();
        }

        // private void asOperator(){
        //     if(symbol.Token == Token.as)
        // }

        private void prefixOperator()
        {
            if (symbol.Token == Token.TokenSubtraction || symbol.Token == Token.TokenNot)
                readNextSymbol();
            else
                error("Esperava um prefixo");
        }

        private void incrementOperator()
        {
            if (symbol.Token == Token.TokenIncrement || symbol.Token == Token.TokenDecrement)
                readNextSymbol();
            else
                error("Esperava um prefixo");
        }
        #endregion

        #region Operations Expression

        #region Logical
        private void logicalOrExpression()
        {
            logicalAndExpression();
            while (symbol.Token == Token.TokenOr)
            {
                readNextSymbol();
                logicalAndExpression();
            }
        }

        private void logicalAndExpression()
        {
            equalityExpression();
            while (symbol.Token == Token.TokenAnd)
            {
                readNextSymbol();
                equalityExpression();
            }
        }
        #endregion


        #region Relational

        private void equalityExpression()
        {
            relationalExpression();
            if (isEqualityOperator(symbol.Token))
            {
                readNextSymbol();
                relationalExpression();
            }
        }

        private void relationalExpression()
        {
            bitwiseOrExpression();
            if (symbol.Token == Token.TokenKeywordIs)
                typeTest();
            else if (isRelationalOperator(symbol.Token))
            {
                readNextSymbol();
                bitwiseOrExpression();
            }
        }

        private void typeTest()
        {
            isOperator();
            var statementParser = new StatementsParser();
            symbol = statementParser.ParseType(symbol);
        }
        #endregion
        #region Bitwise


        private void bitwiseOrExpression()
        {
            bitwiseXorExpression();
            while (symbol.Token == Token.TokenBitwiseOR)
            {
                readNextSymbol();
                bitwiseXorExpression();
            }
        }

        private void bitwiseXorExpression()
        {
            bitwiseAndExpression();
            while (symbol.Token == Token.TokenBitwiseNot)
            {
                readNextSymbol();
                bitwiseAndExpression();
            }
        }

        private void bitwiseAndExpression()
        {
            shiftExpression();
            while (symbol.Token == Token.TokenBitwiseAnd)
            {
                readNextSymbol();
                shiftExpression();
            }
        }

        private void shiftExpression()
        {
            additiveExpression();
            while (symbol.Token == Token.TokenLeftShift || symbol.Token == Token.TokenRightShift)
            {
                readNextSymbol();
                additiveExpression();
            }
        }
        #endregion

        #region Arithmetic
        private void additiveExpression()
        {
            multiplicativeExpression();
            while (symbol.Token == Token.TokenAdition || symbol.Token == Token.TokenSubtraction)
            {
                readNextSymbol();
                multiplicativeExpression();
            }
        }

        private void multiplicativeExpression()
        {
            unaryExpression();
            while (symbol.Token == Token.TokenMultiplication || symbol.Token == Token.TokenDivision || symbol.Token == Token.TokenModulus)
            {
                readNextSymbol();
                unaryExpression();
            }
        }
        #endregion

        #region Unary
        private void unaryExpression()
        {
            if (symbol.Token == Token.TokenKeywordAwait)
            {
                readNextSymbol();
                unaryExpression();
            }
            else if (symbol.Token == Token.TokenIncrement || symbol.Token == Token.TokenDecrement)
            {
                incrementOperator();
            }
            else if (symbol.Token == Token.TokenSubtraction || symbol.Token == Token.TokenNot)
            {
                prefixOperator();
                unaryExpression();
            }
            else if (isLiteral(symbol.Token) || symbol.Token == Token.TokenKeywordThis || symbol.Token == Token.TokenKeywordNew || symbol.Token == Token.TokenOpenParenteses || symbol.Token == Token.TokenID)
            {
                postfixExpression();
            }
            else
            {
                selector();
            }
        }

        private void postfixExpression()
        {
            if (symbol.Token == Token.TokenID)
            {
                assignableExpression();
                incrementOperator();
            }
            else
            {
                primary();
                while (symbol.Token == Token.TokenOpenParenteses || symbol.Token == Token.TokenOpenBrackets)
                {
                    selector();
                }
            }
        }

        private void selector()
        {
            if (symbol.Token == Token.TokenOpenParenteses)
            {
                assignableExpression();
            }
            else
            {
                arguments();
            }
        }
        #endregion
        #endregion

        #region Literals

        private bool isLiteral(Token token) => isBooleanLiteral(token) || isNumericLiteral(token) || symbol.Token == Token.TokenKeywordNull || symbol.Token == Token.TokenString || symbol.Token == Token.TokenOpenCBrackets || symbol.Token == Token.TokenOpenBrackets;
        private void literal()
        {
            if (symbol.Token == Token.TokenKeywordNull)
                readNextSymbol();
            else if (isBooleanLiteral(symbol.Token))
                readNextSymbol();
            else if (symbol.Token == Token.TokenString)
                readNextSymbol();
            else if (isNumericLiteral(symbol.Token))
                readNextSymbol();
            else if (symbol.Token == Token.TokenOpenCBrackets)
                mapLiteral();
            else if (symbol.Token == Token.TokenOpenBrackets)
                listLiteral();
            else
                error("Esperava um literal");
        }

        private bool isBooleanLiteral(Token token) => token == Token.TokenKeywordTrue || token == Token.TokenKeywordFalse;

        private bool isNumericLiteral(Token token) => token == Token.TokenInteger || token == Token.TokenFloatingPoint;

        private void mapLiteral()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenCloseCBrackets) { readNextSymbol(); return; }

            mapLiteralEntry();
            while (symbol.Token == Token.TokenComma)
            {
                readNextSymbol();
                mapLiteralEntry();
            }
        }

        private void mapLiteralEntry()
        {
            expression();
            if (symbol.Token == Token.TokenColon)
                readNextSymbol();
            else
                error("Esperava ':'");
            expression();
        }

        private void listLiteral()
        {
            readNextSymbol();
            if (symbol.Token == Token.TokenCloseBrackets) { readNextSymbol(); return; }
            expressions();
        }
        #endregion
    }
}
