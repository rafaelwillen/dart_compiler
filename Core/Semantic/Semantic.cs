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
                else if (currentSymbol.Token == Token.TokenOpenCBrackets)
                    currentScope++;
                else if (currentSymbol.Token == Token.TokenCloseCBrackets)
                    currentScope--;
                else if (currentScope < 0)
                {
                    error("Mau término do escopo. Encontrado '}' a mais");
                }
                else if (isVariableDeclaration(currentSymbol))
                {
                    analyzeVariableDeclaration();
                }
                else if (currentSymbol.Token == Token.TokenID)
                {
                    analyzeAssignment();
                }
                else if (currentSymbol.Token == Token.TokenKeywordIf || currentSymbol.Token == Token.TokenKeywordWhile)
                {
                    analyzeControlStatement();
                }
                else if (currentSymbol.Token == Token.TokenKeywordSwitch)
                {
                    analyzeSwitchStatement();
                }
            }
        }

        private void analyzeCaseStatement()
        {
            readSymbol(); readSymbol();
            if (!isLiteral(currentSymbol.Token))
            {
                error($"Esperava um literal mas foi encontrado '{currentSymbol.Token}'");
                return;
            }

        }

        private void analyzeSwitchStatement()
        {
            readSymbol(); readSymbol();
            if (!variableExists(currentSymbol.Lexeme))
            {
                error($"Variável '{currentSymbol.Lexeme}' não foi declarada");
            }
        }

        private void analyzeControlStatement()
        {
            bool isConditionalExpression = false;
            readSymbol();
            while (currentSymbol.Token != Token.TokenCloseParenteses)
            {
                if (currentSymbol.Token == Token.TokenID)
                {
                    string identifier = currentSymbol.Lexeme;
                    if (!variableExists(identifier))
                    {
                        error($"Variável '{identifier}' não foi declarada");
                    }
                    else if (variableExists(identifier))
                    {
                        var variable = findVariable(identifier);
                        if (variable.DataType == "bool")
                        {
                            isConditionalExpression = true;
                        }
                    }
                }
                else if (isConditionalOperator(currentSymbol.Token))
                {
                    isConditionalExpression = true;
                }
                readSymbol();
            }

            if (!isConditionalExpression)
            {
                error($"Expressão condicional inválida");
            }
        }

        private void analyzeAssignment()
        {
            string varName = currentSymbol.Lexeme;
            if (!variableExists(varName))
            {
                error($"Variável '{varName}' não foi declarada");
                return;
            }
            var variable = findVariable(varName);
            if (variable.IsConstFinal)
            {
                error("Não é possível atribuir valor em uma constante");
                return;
            }

            //TODO: Check type assignment;
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
            var variable = new Variable(varName, varType, currentScope, isConstFinal);
            string valueAssigned = "$";
            if (currentSymbol.Token != Token.TokenAssignment && isConstFinal)
            {
                error("Constante sem valor atribuído");
                return;
            }
            while (currentSymbol.Token != Token.TokenEndStatement)
            {
                //TODO: Check type assignment;
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
                var variable = new Variable(argName, argType, currentScope + 1, false);
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

        private bool variableExists(String varName)
        {
            for (int i = currentScope; i >= 0; i--)
            {
                if (variables.ContainsKey($"{varName}${i}"))
                    return true;
            }
            return false;
        }

        private Variable findVariable(string name)
        {
            for (int i = currentScope; i >= 0; i--)
            {
                if (variables.ContainsKey($"{name}${i}"))
                {
                    return variables[$"{name}${i}"];
                }
            }
            return null;
        }
    }
}