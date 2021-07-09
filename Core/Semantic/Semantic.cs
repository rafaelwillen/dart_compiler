using System;
using System.Collections.Generic;
using dart_compiler.Core.ErrorReport;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Semantic
{
    public class Semantic
    {
        private const int GLOBAL_SCOPE = 0;
        private int currentScope;
        private Symbol currentSymbol;
        private Dictionary<string, Function> functions;
        private Dictionary<string, Variable> variables;

        public Semantic()
        {
            currentScope = GLOBAL_SCOPE;
            functions = new Dictionary<string, Function>();
            variables = new Dictionary<string, Variable>();
        }

        public void StartAnalyzer()
        {
            while (TableSymbol.HasNextSymbol())
            {
                readSymbol();
                if (isFunctionDefinition(currentSymbol))
                {
                    analyzeFunctionSignature();
                }

                if (currentSymbol.Token == Token.TokenOpenCBrackets)
                    currentScope++;

                if (currentSymbol.Token == Token.TokenCloseCBrackets)
                    currentScope--;

                if (currentScope < 0)
                {
                    error("Mau término do escopo. Encontrado '}' a mais");
                }

                if (isVariableDeclaration(currentSymbol))
                {
                    analyzeVariableDeclaration();
                }
            }
        }

        private void analyzeVariableDeclaration()
        {
            string varType = currentSymbol.Lexeme;
            readSymbol();
            bool isConstFinal = varType == "const" || varType == "final";
            if (isConstFinal)
            {
                varType = currentSymbol.Lexeme;
                readSymbol();
            }
            string varName = currentSymbol.Lexeme;
            readSymbol();
            var variable = new Variable(varName, varType, currentScope);
            Console.WriteLine(variable);
            string valueAssigned = "$";
            while (currentSymbol.Token != Token.TokenEndStatement)
            {
                if (currentSymbol.Token == Token.TokenAssignment)
                    readSymbol();
                readSymbol();
            }
            if (valueAssigned == "$")
                variable.ValueAssigned = "NULL";
            else
                variable.ValueAssigned = valueAssigned;
            if (variables.ContainsKey($"{variable.Identifier}${currentScope}"))
            {
                error($"A variável '{variable.Identifier}' já foi declarada nesse escopo");
                return;
            }
            variables.Add($"{variable.Identifier}${currentScope}", variable);

        }

        private void analyzeFunctionSignature()
        {
            string returnType = currentSymbol.Lexeme;
            readSymbol();
            string functionName = currentSymbol.Lexeme;
            var function = new Function(functionName, returnType);
            readSymbol(); readSymbol();
            while (currentSymbol.Token != Token.TokenCloseParenteses)
            {
                string argType = currentSymbol.Lexeme;
                readSymbol();
                string argName = currentSymbol.Lexeme;
                var variable = new Variable(argName, argType, currentScope + 1);
                function.Arguments.Add(variable);
                readSymbol();
            }
            if (functions.ContainsKey(function.Identifier))
            {
                error("Já foi definida uma função com o mesmo identificador");
                return;
            }
            functions.Add(function.Identifier, function);
        }

        private bool isFunctionDefinition(Symbol symbol) => (symbol.isDataType() ||
        symbol.Token == Token.TokenKeywordVoid) && currentScope == GLOBAL_SCOPE;

        private bool isVariableDeclaration(Symbol symbol) => symbol.isDataType() || symbol.Token == Token.TokenKeywordConst || symbol.Token == Token.TokenKeywordFinal;

        private void readSymbol()
        {
            currentSymbol = TableSymbol.GetNextSymbol();
        }

        private void error(String message)
        {
            var error = new Exception(message + ": Linha " + currentSymbol.LineOfCode);
            ErrorList.AddError(error);
        }

        private bool isLiteral(Token token) => token == Token.TokenKeywordTrue || token == Token.TokenKeywordFalse || token == Token.TokenInteger || token == Token.TokenFloatingPoint || token == Token.TokenKeywordNull || token == Token.TokenString || token == Token.TokenOpenCBrackets || token == Token.TokenOpenBrackets;

        private bool isArithmeticOperator(Token token)
        {
            switch (token)
            {
                case Token.TokenAdition:
                case Token.TokenSubtraction:
                case Token.TokenDivision:
                case Token.TokenMultiplication:
                case Token.TokenModulus:
                    return true;
                default:
                    return false;
            }
        }

        private bool isConditionalOperator(Token token)
        {
            switch (token)
            {
                case Token.TokenEqual:
                case Token.TokenGreater:
                case Token.TokenGreaterEqual:
                case Token.TokenLess:
                case Token.TokenLessEqual:
                case Token.TokenNotEqual:
                case Token.TokenKeywordTrue:
                case Token.TokenKeywordFalse:
                    return true;
                default:
                    return false;
            }
        }

        private bool isLogicalOperator(Token token)
        {
            switch (token)
            {
                case Token.TokenNot:
                case Token.TokenOr:
                case Token.TokenAnd:
                case Token.TokenKeywordTrue:
                case Token.TokenKeywordFalse:
                    return true;
                default:
                    return false;
            }
        }

    }
}