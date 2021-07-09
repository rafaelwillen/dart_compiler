using System;
using System.IO;

using dart_compiler.Core.Scanner;
using dart_compiler.Core;
using dart_compiler.Core.ErrorReport;
using dart_compiler.Core.Parser;
using dart_compiler.Core.Semantic;
// using dart_compiler.Core.Parser;

namespace dart_compiler
{
    class Program
    {
        const int MAX_ARGS_COUNT = 2;
        static int Main(string[] args)
        {
            switch (args.Length)
            {
                case < 1:
                    Console.WriteLine("Erro fatal: Não existe o ficheiro de entrada");
                    Console.WriteLine("Uso: DartCompiler <caminho_ficheiro>");
                    return -1;
                case > MAX_ARGS_COUNT:
                    Console.WriteLine("Demasiados argumentos");
                    Console.WriteLine("Uso: DartCompiler <caminho_ficheiro>");
                    return -1;
            }


            if (args[0] == "-h")
            {
                Console.WriteLine("Mini Compilador de Dart");
                Console.WriteLine("DartCompiler <caminho_ficheiro> --> Compila o ficheiro");
                Console.WriteLine("DartCompiler -h --> Lista os comandos do DartCompiler");
                Console.WriteLine("DartCompiler <código_fonte> -s --> Compila o código fonte e imprime apenas a tabela de símbolos do Scanner");
                Console.WriteLine("DartCompiler <código_fonte> --scanner --> Compila o código fonte e imprime apenas a tabela de símbolos do Scanner");
                return 0;
            }
            string filePath = args[0];
            bool showScannerOutput = args.Length == 2 && (args[1] == "-s" || args[1] == "--scanner");

            Scanner scanner;
            try
            {
                scanner = new Scanner(filePath);
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine($"Erro Fatal: Não foi possível encontrar o ficheiro {e.FileName}");
                return -1;
            }

            while (!scanner.EndOfFile)
            {
                var symbol = scanner.Analex();
                if (symbol.isComment()) continue;
                TableSymbol.Insert(symbol);
            }

            if (ErrorList.ExistsErrors())
            {
                ErrorList.PrintErrors();
                return -1;
            }

            Parser parser = new Parser();
            if (showScannerOutput)
                TableSymbol.PrintTable();
            while (TableSymbol.HasNextSymbol())
            {
                parser.StartParsing(null);
            }

            if (ErrorList.ExistsErrors())
            {
                ErrorList.PrintErrors();
                return -1;
            }

            TableSymbol.ResetSymbolIndex();
            Semantic semanticAnaliser = new Semantic();
            semanticAnaliser.StartAnalyzer();

            // Imprime os erros encontrados
            if (ErrorList.ExistsErrors())
            {
                ErrorList.PrintErrors();
                return -1;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Compilado com sucesso!");
            Console.ResetColor();
            return 0;
        }
    }
}
