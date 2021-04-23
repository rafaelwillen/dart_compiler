using System;

namespace dart_compiler.Core.Scanner
{
    /// <summary>
    /// Classe responsável pela exceção caso existe um comentário de linha que chegou até no fim
    /// do ficheiro
    /// </summary>
    public class CommentaryEndOfFile : Exception
    {
        public CommentaryEndOfFile() { }
        public CommentaryEndOfFile(int fileLine) : base($"Erro - Fim do ficheiro no comentário. Comentário começou na linha {fileLine + 1}") { }
        protected CommentaryEndOfFile(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}