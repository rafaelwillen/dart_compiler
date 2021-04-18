using System;
using System.IO;

namespace dart_compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case < 1:
                    Console.WriteLine("Erro fatal: Não existe o ficheiro de entrada");
                    Console.WriteLine("Uso: DartCompiler <caminho_ficheiro>");
                    return;
                case > 1:
                    Console.WriteLine("Demasiados argumentos");
                    Console.WriteLine("Uso: DartCompiler <caminho_ficheiro>");
                    return;
            }

            // Verifica se o ficheiro existe
            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Erro Fatal: Não foi possível encontrar o ficheiro {args[0]}");
                return;
            }

            Console.WriteLine("All Good!");
        }
    }
}
