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

        public override string ToString()
        {
            return $"ID: {Identifier};ReturnType: {ReturnType} with {Arguments.Count} arguments";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Function))
                return false;
            var function = (Function)obj;
            return function.Identifier == this.Identifier;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}