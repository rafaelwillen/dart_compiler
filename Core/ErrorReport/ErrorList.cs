using System;
using System.Collections.Generic;

namespace dart_compiler.Core.ErrorReport
{
    /// <summary>
    /// Responsável por armazenar os erros encontrados no processo de compilação
    /// </summary>
    public static class ErrorList
    {
        private static List<Exception> errors = new List<Exception>();

        /// <summary>
        /// Adiciona um novo erro na lista de erros
        /// </summary>
        /// <param name="error">O erro</param>
        public static void AddError(Exception error)
        {
            errors.Add(error);
        }

        /// <summary>
        /// Imprime todos erros encontrados no processo de compilação
        /// </summary>
        public static void PrintErrors()
        {
            if (!ExistsErrors())
                throw new InvalidOperationException("A lista de erros está vázia");
            Console.ForegroundColor = ConsoleColor.Red;
            if (errors.Count == 1)
                Console.WriteLine($"Encontrado {errors.Count} erro de compilação");
            else
                Console.WriteLine($"Encontrado {errors.Count} erros de compilação");
            errors.ForEach((error) =>
            {
                Console.WriteLine(error.Message);
            });
            Console.ResetColor();
        }

        /// <summary>
        /// Verifica se foram encontrados erros no processo de compilação
        /// </summary>
        /// <returns>True se existirem erros</returns>
        public static bool ExistsErrors()
        {
            return errors.Count > 0;
        }
    }
}