
using System;
using dart_compiler.Core;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Parser
{
    public class Parser
    {
        private Symbol symbol;

        public Parser()
        {

        }

        public void Parse()
        {
            symbol = TableSymbol.GetNextSymbol();
        }

        private void readNextSymbol()
        {
            symbol = TableSymbol.GetNextSymbol();
        }

        private void error(string message)
        {
            Console.WriteLine($"Erro - encontrado caracter {symbol.Token} - {message}");
            Console.ReadLine();
        }

        /*
            S --> Az | z
            A --> xA | B
            B --> y
        */

        // private void S()
        // {
        //     if (symbol.Lexeme == "z") readNextSymbol();
        //     else
        //     {
        //         A();
        //         if (symbol.Lexeme != "z") error("Z expected");
        //         else readNextSymbol();
        //     }
        //     Console.WriteLine("Sucesss!");
        // }

        // private void A()
        // {
        //     if (symbol.Lexeme == "x")
        //     {
        //         readNextSymbol();
        //         A();
        //     }
        //     else B();
        // }

        // private void B()
        // {
        //     if (symbol.Lexeme == "y") readNextSymbol();
        //     else error("Y expected");
        // }
    }
}