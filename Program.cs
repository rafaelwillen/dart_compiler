using System;
using System.IO;

using dart_compiler.Core.Scanner;

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
            string filePath = args[0];

            Scanner scanner;
            try
            {
                scanner = new Scanner(filePath);
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine($"Erro Fatal: Não foi possível encontrar o ficheiro {e.FileName}");
                return;
            }

            while (!scanner.EndOfFile)
            {
                System.Console.WriteLine(scanner.Analex());
            }
            Console.WriteLine("All Good!");
        }
    }
}
