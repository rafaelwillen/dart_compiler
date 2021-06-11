using System;
using System.Collections.Generic;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core
{
    /// <summary>
    /// A estrutura de dados usado ao longo do processo de compilação. Armazena 
    /// <c>Simbolos</c> que serão usados em todas as fases do compilador.
    /// </summary>
    public static class TableSymbol
    {
        // Armazena os símbolos que são criados pelo Scanner do compilador
        private static List<Symbol> symbols = new List<Symbol>();
        private static int currentSymbolPosition = -1;

        /// <summary>
        /// Insere um novo símbolo no fim da tabela de símbolos
        /// </summary>
        /// <param name="symbol">O novo símbolo a ser inserido</param>
        public static void Insert(Symbol symbol)
        {
            symbols.Add(symbol);
        }

        /// <summary>
        /// Imprime todos os símbolos da tabela de símbolos de maneira formatada
        /// </summary>
        public static void PrintTable()
        {
            int tableWidth = 100;
            printLine(tableWidth);
            printRow(tableWidth, "Lexema", "Token", "Linha");
            printLine(tableWidth);
            foreach (Symbol symbol in symbols)
            {
                printRow(tableWidth, symbol.Lexeme, symbol.Token.ToString(), symbol.LineOfCode.ToString());
            }
            printLine(tableWidth);
        }

        /// <summary>
        /// Avança o ponteiro da posição onde o último símbolo lido se encontra e retorna o próximo símbolo 
        /// </summary>
        /// <returns>O próximo símbolo</returns>
        public static Symbol GetNextSymbol()
        {
            if (currentSymbolPosition == symbols.Count - 1)
            {
                throw new IndexOutOfRangeException("There is no more symbols to read on the symbol table");
            }
            return symbols[++currentSymbolPosition];
        }

        /// <summary>
        /// Verifica se ainda existem símbolos a serem lidos
        /// </summary>
        /// <returns>true se ainda existem símbolos para serem lidos</returns>
        public static bool HasNextSymbol()
        {
            return currentSymbolPosition != symbols.Count - 1;
        }

        private static void printLine(int tableWidth) => Console.WriteLine(new String('-', tableWidth));

        private static void printRow(int tableWidth, params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";
            foreach (string column in columns)
            {
                row += alignCenter(column, width) + "|";
            }
            Console.WriteLine(row);
        }

        private static string alignCenter(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}