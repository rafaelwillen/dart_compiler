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
    }
}