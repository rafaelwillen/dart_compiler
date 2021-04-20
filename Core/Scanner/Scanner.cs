using System;
using System.IO;
using System.Collections.Generic;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Scanner
{
    /// <summary>
    /// O analisador léxico
    /// </summary>
    public class Scanner
    {

        public bool EndOfFile { get; private set; }

        private char ch;
        private int charPointer;
        private int linePointer;
        private List<string> buffer;

        public Scanner(string filePath)
        {
            buffer = new List<string>();
            buffer.AddRange(File.ReadAllLines(filePath));
            charPointer = 0;
            linePointer = 0;
            EndOfFile = false;
            ch = getChar();
        }

        public Token Analex()
        {
            if (ch == '\0')
            {
                EndOfFile = true;
                return Token.TokenEOF;
            }
            Token lexToken = Token.TokenInvalid;
            // Ignorar espaços em branco
            while (isWhiteSpace(ch)) ch = getChar();
            // Verificar tokens de um caractere
            lexToken = verifySingleCharacterToken();
            if (lexToken != Token.TokenInvalid) return lexToken;
            // Verificar tokens de dois ou três caracteres
            lexToken = verifyCharacterToken();

            // Verificar token identificador ou keyword
            if (isLetter(ch) || ch == '$' || ch == '_')
            {
                string identifier = string.Empty;
                while (isLetter(ch) || ch == '$' || ch == '_' || isDigit(ch))
                {
                    identifier += ch;
                    ch = getChar();
                }
                lexToken = isKeyword(identifier) ? Token.TokenKeyword : Token.TokenID;
            }

            // Verificar token inteiro ou hexadecimal
            if (isDigit(ch))
            {
                // TODO: Reconhecer números hexadecimais
                int value = 0;
                while (isDigit(ch))
                {
                    value = value * 10 + ch - '0';
                    ch = getChar();
                }
                lexToken = Token.TokenInteger;
            }

            if (lexToken == Token.TokenInvalid)
            {
                ch = getChar();
            }

            return lexToken;
        }

        /// <summary>
        /// Verfica o caractere se é um token de dois ou três caracteres. Se for de um character,
        /// chamar outra função
        /// </summary>
        /// <returns>Retorna o <c>Token</c> para o caractere lido. Retorna <c>Token.Invalid</c> se não for  
        /// caractere válido neste contexto
        /// </returns>
        private Token verifyCharacterToken()
        {
            Token lexToken = Token.TokenInvalid;
            switch (ch)
            {
                // Analisar <= , <, <<= ou  <<
                case '<':
                    ch = getChar();
                    if (ch == '<')
                    {
                        // Possível << ou <<=
                        ch = getChar();
                        if (ch == '=')
                        {
                            lexToken = Token.TokenCA_LShift;
                            ch = getChar();
                        }
                        else lexToken = Token.TokenLeftShift;
                    }
                    else if (ch == '=')
                    {
                        lexToken = Token.TokenLessEqual;
                        ch = getChar();
                    }
                    // Exato <
                    else lexToken = Token.TokenLess;
                    break;
                // Analisar >=, >, >>= ou  >>
                case '>':
                    ch = getChar();
                    if (ch == '=')
                    {
                        lexToken = Token.TokenGreaterEqual;
                        ch = getChar();
                    }
                    else if (ch == '>')
                    {
                        ch = getChar();
                        // Possível >>= ou >>
                        if (ch == '=')
                        {
                            lexToken = Token.TokenCA_RShift;
                        }
                        else lexToken = Token.TokenRightShift;
                    }
                    else lexToken = Token.TokenGreater;
                    break;
                // Analisar = ou ==
                case '=':
                    ch = getChar();
                    if (ch == '=')
                    {
                        lexToken = Token.TokenEqual;
                        ch = getChar();
                    }
                    else lexToken = Token.TokenAssignment;
                    break;
                // Analisar != ou ==
                case '!':
                    ch = getChar();
                    if (ch == '=')
                    {
                        lexToken = Token.TokenNotEqual;
                        ch = getChar();
                    }
                    else lexToken = Token.TokenNot;
                    break;
                // Analisar ||, |= ou |
                case '|':
                    ch = getChar();
                    if (ch == '|')
                    {
                        lexToken = Token.TokenOr;
                        ch = getChar();
                    }
                    else if (ch == '=')
                    {
                        lexToken = Token.TokenCA_OR;
                        ch = getChar();
                    }
                    else lexToken = Token.TokenBitwiseOR;
                    break;
                // Analisar &&, & ou &=
                case '&':
                    ch = getChar();
                    if (ch == '&')
                    {
                        ch = getChar();
                        lexToken = Token.TokenAnd;
                    }
                    else if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCA_AND;
                    }
                    else lexToken = Token.TokenBitwiseAnd;
                    break;
                // Analisar +, ++, +=
                case '+':
                    ch = getChar();
                    if (ch == '+')
                    {
                        ch = getChar();
                        lexToken = Token.TokenIncrement;
                    }
                    else if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCAAdition;
                    }
                    else lexToken = Token.TokenAdition;
                    break;
                // Analisar -, --, -=
                case '-':
                    ch = getChar();
                    if (ch == '-')
                    {
                        ch = getChar();
                        lexToken = Token.TokenDecrement;
                    }
                    else if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCASubtraction;
                    }
                    else lexToken = Token.TokenSubtraction;
                    break;
                // Analisar / , /= , // ou /*
                case '/':
                    ch = getChar();
                    // Start of the comment
                    if (ch == '/')
                    {
                        bool inComment = true;
                        ch = getChar();
                        while (inComment)
                        {
                            if (ch == '\0')
                            {
                                inComment = false;
                            }
                            else if (ch == '\n')
                            {
                                inComment = false;
                            }
                            else ch = getChar();
                        }
                        lexToken = Analex();
                    }
                    else if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCADivison;
                    }
                    // Start of the comment
                    else if (ch == '*')
                    {
                        bool inComment = true;
                        ch = getChar();
                        while (inComment)
                        {
                            if (ch == '\0')
                            {
                                Console.Error.WriteLine("Alerta - end of file no comentário");
                                inComment = false;
                            }
                            else if (ch == '*')
                            {
                                ch = getChar();
                                if (ch == '/')
                                {
                                    ch = getChar();
                                    inComment = false;
                                }
                            }
                            else ch = getChar();
                        }
                        lexToken = Analex();
                    }
                    else lexToken = Token.TokenDivision;
                    break;
                // Analisar * , *=
                case '*':
                    ch = getChar();
                    if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCAMultiplication;
                    }
                    else lexToken = Token.TokenMultiplication;
                    break;
                // Analisar %, %=
                case '%':
                    ch = getChar();
                    if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCAModulus;
                    }
                    else lexToken = Token.TokenModulus;
                    break;
                // Analisar ^, ^=
                case '^':
                    ch = getChar();
                    if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCA_NOT;
                    }
                    else lexToken = Token.TokenBitwiseNot;
                    break;
                case '"':
                    int lineDiference = linePointer;
                    string stringLiteral = ch.ToString();
                    ch = getChar();
                    while (ch != '"')
                    {
                        stringLiteral += ch;
                        ch = getChar();
                        if (linePointer > lineDiference)
                        {
                            lexToken = Token.TokenInvalid;
                            break;
                        }
                        lexToken = Token.TokenString;
                    }
                    stringLiteral += ch.ToString();
                    ch = getChar();
                    break;
            }
            return lexToken;
        }

        /// <summary>
        /// Verfica o caractere se é um token de um único caractere
        /// </summary>
        /// <returns>Retorna o <c>Token</c> para o caractere lido. Retorna <c>Token.Invalid</c> se não for  
        /// caractere válido neste contexto
        /// </returns>
        private Token verifySingleCharacterToken()
        {
            Token lexToken = Token.TokenInvalid;
            switch (ch)
            {
                case ';': lexToken = Token.TokenEndStatement; ch = getChar(); break;
                case ',': lexToken = Token.TokenComma; ch = getChar(); break;
                case '^': lexToken = Token.TokenBitwiseNot; ch = getChar(); break;
                case '(': lexToken = Token.TokenOpenParenteses; ch = getChar(); break;
                case ')': lexToken = Token.TokenCloseParenteses; ch = getChar(); break;
                case '{': lexToken = Token.TokenOpenCBrackets; ch = getChar(); break;
                case '}': lexToken = Token.TokenCloseCBrackets; ch = getChar(); break;
                case '[': lexToken = Token.TokenOpenBrackets; ch = getChar(); break;
                case ']': lexToken = Token.TokenCloseBrackets; ch = getChar(); break;
                case '\0': lexToken = Token.TokenEOF; break;
            }
            return lexToken;
        }

        /// <summary>
        /// Verifica se o identificador é uma palavra reservada
        /// </summary>
        /// <param name="identifier">O identificador a ser verificado</param>
        /// <returns>true se for uma palavra reservada</returns>
        private bool isKeyword(string identifier)
        {
            string[] keywords = {
                "assert", "break", "case", "catch", "class", "const", "continue",
                "default", "do", "else", "enum", "extends", "false", "final", "finally",
                "for", "if", "in", "is", "new", "null", "rethrow", "return", "super",
                "switch", "this", "throw", "true", "try", "var", "void", "while", "with",
                 "int", "double", "List", "Map", "String", "Object", "dynamic"
            };
            return Array.Exists(keywords, keyword => keyword.Equals(identifier));
        }

        /// <summary>
        /// Verifica se o caractere é um espaço em branco
        /// </summary>
        /// <param name="ch">O caractere a ser analisado</param>
        /// <returns>true se for um espaço em branco</returns>
        private bool isWhiteSpace(char ch) => ch == ' ';

        /// <summary>
        /// Verifica se o caractere é uma letra
        /// </summary>
        /// <param name="ch">O caractere a ser analisado</param>
        /// <returns>true se for uma letra</returns>
        private bool isLetter(char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');

        /// <summary>
        /// Verifica se o caractere é um dígito
        /// </summary>
        /// <param name="ch">O caractere a ser analisado</param>
        /// <returns>true se for um dígito</returns>
        private bool isDigit(char ch) => ch >= '0' && ch <= '9';

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        private char getChar()
        {
            if (EndOfFile)
            {
                throw new EndOfStreamException("Fim do ficheiro. Mais nada para ler");
            }

            if (charPointer == buffer[buffer.Count - 1].Length && linePointer == buffer.Count - 1) return '\0';

            if (charPointer == buffer[linePointer].Length)
            {
                linePointer++;
                charPointer = 0;
            }
            return buffer[linePointer][charPointer++];
        }
    }
}