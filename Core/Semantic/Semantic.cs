using System;
using System.Collections.Generic;
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
            }
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
        }

        private bool isFunctionDefinition(Symbol symbol) => symbol.isDataType() ||
        symbol.Token == Token.TokenKeywordVoid || currentScope == GLOBAL_SCOPE;

        private void readSymbol()
        {
            currentSymbol = TableSymbol.GetNextSymbol();
        }
    }
}