using System;
using System.IO;

using dart_compiler.Core.Scanner;
using dart_compiler.Core;
using dart_compiler.Core.ErrorReport;
using dart_compiler.Core.Parser;

namespace dart_compiler
{
    class Program
    {
        const int MAX_ARGS_COUNT = 2;
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case < 1:
                    Console.WriteLine("Erro fatal: Não existe o ficheiro de entrada");
                    Console.WriteLine("Uso: DartCompiler <caminho_ficheiro>");
                    return;
                case > MAX_ARGS_COUNT:
                    Console.WriteLine("Demasiados argumentos");
                    Console.WriteLine("Uso: DartCompiler <caminho_ficheiro>");
                    return;
            }


            if (args[0] == "-h")
            {
                Console.WriteLine("Mini Compilador de dart");
                Console.WriteLine("DartCompiler <caminho_ficheiro> --> Compila o ficheiro");
                Console.WriteLine("DartCompiler -h --> Lista de comandos do DartCompiler");
                Console.WriteLine("DartCompiler <código_fonte> -f --> Compila o código fonte e armazena os resultados em um ficheiro de saída a.txt");
                Console.WriteLine("DartCompiler <código_fonte> -f <ficheiro_saida> --> Compila o código fonte e armazena os resultados em um ficheiro de saída");
                Console.WriteLine("DartCompiler <código_fonte> -s --> Compila o código fonte e imprime apenas a tabela de símbolos do Scanner");
                Console.WriteLine("DartCompiler <código_fonte> --scanner --> Compila o código fonte e imprime apenas a tabela de símbolos do Scanner");
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
                var symbol = scanner.Analex();
                if (symbol.isComment()) continue;
                TableSymbol.Insert(symbol);
            }

            // Imprime os erros encontrados na etapa do Scanner
            if (ErrorList.ExistsErrors())
            {
                Console.WriteLine("Erro no Scanner");
                ErrorList.PrintErrors();
                return;
            }
            TableSymbol.PrintTable();
            Parser parser = new Parser();
            parser.Parse();
        }
    }
}
