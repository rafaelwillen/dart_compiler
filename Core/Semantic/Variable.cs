namespace dart_compiler.Core.Semantic
{
    public class Variable
    {
        public string Identifier { get; set; }

        public string DataType { get; set; }

        public string ValueAssigned { get; set; }

        public int Scope { get; set; }

        public Variable(string identifier, string dataType, int scopeLevel)
        {
            Identifier = identifier;
            DataType = dataType;
            Scope = scopeLevel;
            ValueAssigned = string.Empty;
        }

        public Variable(string identifier, string dataType, int scopeLevel, string valueAssigned)
        {
            Identifier = identifier;
            DataType = dataType;
            Scope = scopeLevel;
            ValueAssigned = valueAssigned;
        }

        public override string ToString()
        {
            return $"ID: {Identifier};Type: {DataType};ScopeLevel: {Scope}";
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Variable))
                return false;
            var variable = (Variable)obj;
            return variable.Identifier == this.Identifier;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}