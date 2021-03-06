using System;
namespace dart_compiler.Core.Utils
{

    /// <summary>
    /// Forma de agrupar os componentes que serão criados e usados no processo de compilação
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Sequência de caracteres obtidas no código fonte que correspondem a um padrão
        /// de um token. É a instância do token
        /// </summary>
        /// <value></value>
        public string Lexeme { get; set; }
        /// <summary>
        /// Nome abstrato de um símbolo que representa um tipo de unidade léxica
        /// </summary>
        /// <value></value>
        public Token Token { get; set; }
        /// <summary>
        /// A linha do código fonte onde foi encontrado o símbolo 
        /// </summary>
        /// <value></value>
        public int LineOfCode { get; private set; }

        public Symbol(string lexeme, Token token, int lineOfCode)
        {
            Lexeme = lexeme;
            Token = token;
            LineOfCode = lineOfCode;
        }

        /// <summary>
        /// Verifica se este símbolo é um comentário de linha ou múltiplas linhas
        ///</summary>
        /// <returns>true se for um comentário</returns>
        public bool isComment() => Token == Token.TokenComment;

        /// <summary>
        /// Verifica se este símbolo é um tipo de dados
        /// Tipos de dados aceites: int, double, String, dynamic, Object, List e Map
        /// </summary>
        /// <returns>true se for um tipo de dados</returns>
        public bool isDataType()
        {
            string[] dataTypes = {
                "int", "double", "String", "dynamic", "Object", "List", "Map"
            };

            return Array.Exists<string>(dataTypes, (dataType) => dataType == Lexeme);
        }

        public override string ToString()
        {
            return $"{this.Token} : {Lexeme} at line {LineOfCode}";
        }
    }
}