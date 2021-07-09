using System;
using System.Collections.Generic;

namespace dart_compiler.Core.Semantic
{
    public class Function
    {
        public string Identifier { get; set; }

        public List<Variable> Arguments { get; set; }

        public string ReturnType { get; set; }

        public bool IsVoid => ReturnType.Equals("void");

        public Function(string identifier, string returnType)
        {
            Identifier = identifier;
            Arguments = new List<Variable>();
            ReturnType = returnType;
        }
    }
}