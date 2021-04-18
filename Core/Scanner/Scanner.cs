using System;
using System.IO;
using dart_compiler.Core.Utils;

namespace dart_compiler.Core.Scanner
{
    /// <summary>
    /// O analisador léxico
    /// </summary>
    public class Scanner
    {

        private char ch;
        public Token lex()
        {
            Token lexToken = Token.TokenInvalid;
            // Ignorar espaços em branco
            while (isWhiteSpace(ch)) ch = getChar();
            // Verificar tokens de um caractere
            lexToken = verifySingleCharacterToken();
            // Verificar tokens de dois ou três caracteres
            lexToken = verifyCharacterToken();

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
                    if (ch == '/')
                    {
                        ch = getChar();
                        // TODO: Verificar comentário de uma linha
                    }
                    else if (ch == '=')
                    {
                        ch = getChar();
                        lexToken = Token.TokenCADivison;
                    }
                    else if (ch == '*')
                    {
                        ch = getChar();
                        // TODO: Verificar comentário de várias linhas
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
        /// Verifica se o caractere é um espaço em branco
        /// </summary>
        /// <param name="ch">O caractere a ser analisado</param>
        /// <returns>true se for um espaço em branco</returns>
        private bool isWhiteSpace(char ch) => ch == ' ';

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        private char getChar()
        {
            throw new NotImplementedException();
        }
    }
}