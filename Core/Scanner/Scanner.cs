using System;
using System.IO;
using System.Collections.Generic;
using dart_compiler.Core.Utils;
using dart_compiler.Core.ErrorReport;
using dart_compiler.Core.ErrorReport.ScannerError;

namespace dart_compiler.Core.Scanner
{
    /// <summary>
    /// O analisador léxico
    /// </summary>
    public class Scanner
    {
        private const string KEY_TOKEN = "KEY_Token";
        private const string KEY_LEXEME = "KEY_Lexeme";

        public bool EndOfFile { get; private set; }

        private char ch;
        private int charPointer;
        private int linePointer;
        private List<string> buffer;

        public Scanner(string filePath)
        {
            buffer = new List<string>();
            buffer.AddRange(Array.ConvertAll<string, string>(File.ReadAllLines(filePath), line => line.Trim()));
            charPointer = 0;
            linePointer = 0;
            EndOfFile = false;
            ch = getChar();
        }

        public Symbol Analex()
        {
            if (ch == '\0')
            {
                EndOfFile = true;
                return new Symbol("", Token.TokenEOF, linePointer + 1);
            }
            Token lexToken = Token.TokenInvalid;
            Dictionary<string, string> result = new Dictionary<string, string>();
            // Ignorar espaços em branco
            while (isWhiteSpace(ch)) ch = getChar();
            // Verificar tokens de um caractere
            result = verifySingleCharacterToken();
            Enum.TryParse(result[KEY_TOKEN], out lexToken);
            if (result[KEY_TOKEN] != Token.TokenInvalid.ToString()) return new Symbol(result[KEY_LEXEME], lexToken, linePointer + 1);
            // Verificar tokens de dois ou três caracteres
            result = verifyCharacterToken();
            Enum.TryParse(result[KEY_TOKEN], out lexToken);
            // Verificar token identificador ou keyword
            if (isLetter(ch) || ch == '$' || ch == '_')
            {
                string identifier = string.Empty;
                while (isLetter(ch) || ch == '$' || ch == '_' || isDigit(ch))
                {
                    identifier += ch;
                    ch = getChar();
                }
                result[KEY_LEXEME] = identifier;
                if (isKeyword(identifier))
                {
                    foreach (Token token in Enum.GetValues<Token>())
                    {
                        if (!token.ToString().StartsWith("TokenKeyword"))
                        {
                            continue;
                        }
                        string stringToken = token.ToString().Substring(12).ToLower();
                        if (stringToken.Equals(identifier))
                        {
                            lexToken = token;
                            break;
                        }
                    }
                }
                else
                {
                    lexToken = Token.TokenID;
                }
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
                result[KEY_LEXEME] = value.ToString();
                lexToken = Token.TokenInteger;
            }

            if (lexToken == Token.TokenInvalid)
            {
                ch = getChar();
            }

            return new Symbol(result[KEY_LEXEME], lexToken, linePointer + 1);
        }

        /// <summary>
        /// Verfica o caractere se é um token de dois ou três caracteres. Se for de um character,
        /// chamar outra função
        /// </summary>
        /// <returns>Retorna o <c>Token</c> para o caractere lido. Retorna <c>Token.Invalid</c> se não for  
        /// caractere válido neste contexto
        /// </returns>
        private Dictionary<string, string> verifyCharacterToken()
        {
            Token lexToken = Token.TokenInvalid;
            string lexeme = string.Empty;
            switch (ch)
            {
                // Analisar <= , <, <<= ou  <<
                case '<':
                    lexeme += ch.ToString();
                    ch = getChar();
                    if (ch == '<')
                    {
                        // Possível << ou <<=
                        lexeme += ch.ToString();
                        ch = getChar();
                        if (ch == '=')
                        {
                            lexToken = Token.TokenCA_LShift;
                            lexeme += ch.ToString();
                            ch = getChar();
                        }
                        else lexToken = Token.TokenLeftShift;
                    }
                    else if (ch == '=')
                    {
                        lexToken = Token.TokenLessEqual;
                        lexeme += ch.ToString();
                        ch = getChar();
                    }
                    // Exato <
                    else lexToken = Token.TokenLess;
                    break;
                // Analisar >=, >, >>= ou  >>
                case '>':
                    lexeme += ch.ToString();
                    ch = getChar();
                    if (ch == '=')
                    {
                        lexToken = Token.TokenGreaterEqual;
                        lexeme += ch.ToString();
                        ch = getChar();
                    }
                    else if (ch == '>')
                    {
                        lexeme += ch.ToString();
                        ch = getChar();
                        // Possível >>= ou >>
                        if (ch == '=')
                        {
                            lexeme += ch.ToString();
                            lexToken = Token.TokenCA_RShift;
                            ch = getChar();
                        }
                        else lexToken = Token.TokenRightShift;
                    }
                    else lexToken = Token.TokenGreater;
                    break;
                // Analisar = ou ==
                case '=':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        lexToken = Token.TokenEqual;
                        ch = getChar();
                    }
                    else lexToken = Token.TokenAssignment;
                    break;
                // Analisar != ou ==
                case '!':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        lexToken = Token.TokenNotEqual;
                        ch = getChar();
                    }
                    else lexToken = Token.TokenNot;
                    break;
                // Analisar ||, |= ou |
                case '|':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '|')
                    {
                        lexeme += ch.ToString();

                        lexToken = Token.TokenOr;
                        ch = getChar();
                    }
                    else if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        lexToken = Token.TokenCA_OR;
                        ch = getChar();
                    }
                    else lexToken = Token.TokenBitwiseOR;
                    break;
                // Analisar &&, & ou &=
                case '&':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '&')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenAnd;
                    }
                    else if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenCA_AND;
                    }
                    else lexToken = Token.TokenBitwiseAnd;
                    break;
                // Analisar +, ++, +=
                case '+':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '+')
                    {
                        lexeme += ch.ToString();
                        ch = getChar();
                        lexToken = Token.TokenIncrement;
                    }
                    else if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenCAAdition;
                    }
                    else lexToken = Token.TokenAdition;
                    break;
                // Analisar -, --, -=
                case '-':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '-')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenDecrement;
                    }
                    else if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenCASubtraction;
                    }
                    else lexToken = Token.TokenSubtraction;
                    break;
                // Analisar / , /= , // ou /*
                case '/':
                    lexeme += ch.ToString();
                    ch = getChar();
                    // Start of the comment
                    if (ch == '/')
                    {
                        bool inComment = true;
                        // Usado para verificar se mudou de linha
                        int commentLineStart = linePointer;
                        ch = getChar();
                        while (inComment)
                        {
                            if (ch == '\0' || commentLineStart < linePointer)
                            {
                                inComment = false;
                                lexToken = Token.TokenComment;
                            }
                            else ch = getChar();
                        }
                    }
                    else if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCADivision;
                    }
                    // Start of the comment
                    else if (ch == '*')
                    {
                        bool inComment = true;
                        int commentLineStart = linePointer;
                        ch = getChar();
                        while (inComment)
                        {
                            if (ch == '\0')
                            {
                                inComment = false;
                                ErrorList.AddError(new CommentaryEndOfFileException(commentLineStart));
                            }
                            else if (ch == '*')
                            {
                                ch = getChar();
                                if (ch == '/')
                                {
                                    ch = getChar();
                                    inComment = false;
                                    lexToken = Token.TokenComment;
                                }
                            }
                            else ch = getChar();
                        }
                    }
                    else lexToken = Token.TokenDivision;
                    break;
                // Analisar * , *=
                case '*':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '=')
                    {
                        lexeme += ch.ToString();
                        ch = getChar();
                        lexToken = Token.TokenCAMultiplication;
                    }
                    else lexToken = Token.TokenMultiplication;
                    break;
                // Analisar %, %=
                case '%':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenCAModulus;
                    }
                    else lexToken = Token.TokenModulus;
                    break;
                // Analisar ^, ^=
                case '^':
                    lexeme += ch.ToString();

                    ch = getChar();
                    if (ch == '=')
                    {
                        lexeme += ch.ToString();

                        ch = getChar();
                        lexToken = Token.TokenCA_NOT;
                    }
                    else lexToken = Token.TokenBitwiseNot;
                    break;
                // Analisar string com aspas
                case '"':
                    int lineDiference = linePointer;
                    string stringLiteral = ch.ToString();
                    ch = getChar();
                    while (ch != '"')
                    {
                        stringLiteral += ch;
                        ch = getChar();
                        // Se mudou de linha, então a string não terminou
                        if (linePointer > lineDiference)
                        {
                            ErrorList.AddError(new InvalidStringLiteralException(linePointer, buffer[linePointer - 1]));
                            break;
                        }
                        lexToken = Token.TokenString;
                    }
                    stringLiteral += ch.ToString();
                    lexeme = stringLiteral;
                    ch = getChar();
                    break;
                // Analisar string com virgulas altas
                case '\'':
                    lineDiference = linePointer;
                    stringLiteral = ch.ToString();
                    ch = getChar();
                    while (ch != '\'')
                    {
                        stringLiteral += ch;
                        ch = getChar();
                        // Se mudou de linha, então a string não terminou
                        if (linePointer > lineDiference)
                        {
                            ErrorList.AddError(new InvalidStringLiteralException(linePointer, buffer[linePointer - 1]));
                            break;
                        }
                        lexToken = Token.TokenString;
                    }
                    stringLiteral += ch.ToString();
                    lexeme = stringLiteral;
                    ch = getChar();
                    break;

            }
            return new Dictionary<string, string>() {
                {KEY_LEXEME, lexeme},
                {KEY_TOKEN, Enum.GetName(lexToken)}
            };
        }

        /// <summary>
        /// Verfica o caractere se é um token de um único caractere
        /// </summary>
        /// <returns>Retorna o <c>Token</c> para o caractere lido. Retorna <c>Token.Invalid</c> se não for  
        /// caractere válido neste contexto
        /// </returns>
        private Dictionary<string, string> verifySingleCharacterToken()
        {
            Token lexToken = Token.TokenInvalid;
            string lexeme = ch.ToString();
            switch (ch)
            {
                case ';': lexToken = Token.TokenEndStatement; ch = getChar(); break;
                case ',': lexToken = Token.TokenComma; ch = getChar(); break;
                case '.': lexToken = Token.TokenDot; ch = getChar(); break;
                case '?': lexToken = Token.TokenInterrogation; ch = getChar(); break;
                case '(': lexToken = Token.TokenOpenParenteses; ch = getChar(); break;
                case ')': lexToken = Token.TokenCloseParenteses; ch = getChar(); break;
                case '{': lexToken = Token.TokenOpenCBrackets; ch = getChar(); break;
                case '}': lexToken = Token.TokenCloseCBrackets; ch = getChar(); break;
                case '[': lexToken = Token.TokenOpenBrackets; ch = getChar(); break;
                case ']': lexToken = Token.TokenCloseBrackets; ch = getChar(); break;
            }
            return new Dictionary<string, string>() {
                {KEY_LEXEME, lexeme},
                {KEY_TOKEN, Enum.GetName(lexToken)}
            }; ;
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
                 "Object", "dynamic", "import"
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
        /// Move o ponteiro do caractere e retorna o caractere lido na posição anterior
        /// </summary>
        /// <returns>Um caractere do ficheiro</returns>
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
            while (buffer[linePointer].Equals(string.Empty))
            {
                linePointer++;
            }
            return buffer[linePointer][charPointer++];
        }
    }
}