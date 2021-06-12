using System;

namespace dart_compiler.Core.ErrorReport.ScannerError
{
    public class InvalidFloatingPointException : Exception
    {
        /// <summary>
        /// </summary>
        /// <param name="fileLineNumber">O número da linha onde foi encontrado o erro</param>
        /// <param name="fileLine">A string onde está o erro</param>
        /// <returns></returns>
        public InvalidFloatingPointException(int fileLineNumber, string fileLine) :
        base($"{fileLine} - Número decimal inválido {fileLineNumber}")
        { }
    }
}