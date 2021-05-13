using System;

namespace dart_compiler.Core.ErrorReport.ScannerError
{
    /// <summary>
    /// Classe responsável pela exceção caso existe um comentário de linha que chegou até no fim
    /// do ficheiro
    /// </summary>
    public class CommentaryEndOfFileException : Exception
    {
        public CommentaryEndOfFileException(int fileLine) :
        base($"Fim do ficheiro no comentário. Comentário começou na linha {fileLine + 1}")
        { }

    }
}