using System;

namespace dart_compiler.Core.ErrorReport.ScannerError
{
    /// <summary>
    /// Classe responsável pelo exceção caso existe um string que não terminou 
    /// </summary>
    public class InvalidStringLiteralException : Exception
    {
        /// <summary>
        /// </summary>
        /// <param name="fileLineNumber">O número da linha onde foi encontrado o erro</param>
        /// <param name="fileLine">A string onde está o erro</param>
        /// <returns></returns>
        public InvalidStringLiteralException(int fileLineNumber, string fileLine) :
        base($"{fileLine} - Esperava fim das aspas na linha {fileLineNumber}")
        { }
    }
}