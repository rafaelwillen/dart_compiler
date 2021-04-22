namespace dart_compiler.Core.Utils
{
    /// <summary>
    /// Enumeração que armazena os códigos dos tokens
    /// </summary>
    public enum Token
    {
        /// <summary>
        /// Token Identificador (variáveis e funções)
        /// </summary>
        TokenID,
        /// <summary>
        /// Token número inteiro ou hexadecimal
        /// </summary>
        TokenInteger,
        /// <summary>
        /// Token constante string
        /// </summary>
        TokenString,
        /// <summary>
        /// Token número com ponto flutuante ou notação científica
        /// </summary>
        TokenFloatingPoint,
        /// <summary>
        /// Token operador lógico Ou
        /// </summary>
        TokenOr,
        /// <summary>
        /// Token operador lógico E
        /// </summary>
        TokenAnd,
        /// <summary>
        /// Token operador lógico de negação
        /// </summary>
        TokenNot,
        /// <summary>
        /// Token operador relacional maior que
        /// </summary>
        TokenGreater,
        /// <summary>
        /// Token operador relacional maior ou igual que
        /// </summary>
        TokenGreaterEqual,
        /// <summary>
        /// Token operador relacional de igualdade
        /// </summary>
        TokenEqual,
        /// <summary>
        /// Token operador relacional menor que
        /// </summary>
        TokenLess,
        /// <summary>
        /// Token operador relacional menor ou igual que
        /// </summary>
        TokenLessEqual,
        /// <summary>
        /// Token operador relacional diferença
        /// </summary>
        TokenNotEqual,
        /// <summary>
        /// Token operador aritmético de divisão
        /// </summary>
        TokenDivision,
        /// <summary>
        /// Token operador aritmético de multiplicação
        /// </summary>
        TokenMultiplication,
        /// <summary>
        /// Token operador aritmético de subtração
        /// </summary>
        TokenSubtraction,
        /// <summary>
        /// Token operador aritmético de resto da divisão
        /// </summary>
        TokenModulus,
        /// <summary>
        /// Token operador aritmético de adição
        /// </summary>
        TokenAdition,
        /// <summary>
        /// Token operador aritmético de incremento
        /// </summary>
        TokenIncrement,
        /// <summary>
        /// Token operador aritmético de decremento
        /// </summary>
        TokenDecrement,
        /// <summary>
        /// Token operador de atribuição 
        /// </summary>
        TokenAssignment,
        /// <summary>
        /// Token operador de atribuição divisão
        /// </summary>
        TokenCADivison,
        /// <summary>
        /// Token operador de atribuição multiplicação
        /// </summary>
        TokenCAMultiplication,
        /// <summary>
        /// Token operador de atribuição de resto da divisão
        /// </summary>
        TokenCAModulus,
        /// <summary>
        /// Token operador de atribuição subtração
        /// </summary>
        TokenCASubtraction,
        /// <summary>
        /// Token operador de atribuição adição
        /// </summary>
        TokenCAAdition,
        /// <summary>
        /// Token operador de atribuição deslocamento direita
        /// </summary>
        TokenCA_RShift,
        /// <summary>
        /// Token operador de atribuição deslocamento esquerda
        /// </summary>
        TokenCA_LShift,
        /// <summary>
        /// Token operador de atribuição AND
        /// </summary>
        TokenCA_AND,
        /// <summary>
        /// Token operador de atribuição NOT
        /// </summary>
        TokenCA_NOT,
        /// <summary>
        /// Token operador de atribuição OR
        /// </summary>
        TokenCA_OR,
        /// <summary>
        /// Token operador bitwise deslocamento a direita
        /// </summary>
        TokenRightShift,
        /// <summary>
        /// Token operador bitwise deslocament a esquerda
        /// </summary>
        TokenLeftShift,
        /// <summary>
        /// Token operador bitwise AND
        /// </summary>
        TokenBitwiseAnd,
        /// <summary>
        /// Token operador bitwise NOT
        /// </summary>
        TokenBitwiseNot,
        /// <summary>
        /// Token operador bitwise OR
        /// </summary>
        TokenBitwiseOR,
        /// <summary>
        /// Token palavra reservada
        /// </summary>
        TokenKeyword,
        /// <summary>
        /// Token fim de expressão
        /// </summary>
        TokenEndStatement,
        /// <summary>
        /// Token abrir parenteses
        /// </summary>
        TokenOpenParenteses,
        /// <summary>
        /// Token fechar parenteses
        /// </summary>
        TokenCloseParenteses,
        /// <summary>
        /// Token abrir chavetas
        /// </summary>
        TokenOpenCBrackets,
        /// <summary>
        /// Token fechar chavetas
        /// </summary>
        TokenCloseCBrackets,
        /// <summary>
        /// Token virgula
        /// </summary>
        TokenComma,
        /// <summary>
        /// Token ponto
        /// </summary>
        TokenDot,
        /// <summary>
        /// Token ponto interrogação
        /// </summary>
        TokenInterrogation,
        /// <summary>
        /// Token abrir parenteses rectos
        /// </summary>
        TokenOpenBrackets,
        /// <summary>
        /// Token fechar parenteses rectos
        /// </summary>
        TokenCloseBrackets,
        /// <summary>
        /// Token fim do ficheiro
        /// </summary>
        TokenEOF,
        /// <summary>
        /// Token inválido
        /// </summary>
        TokenInvalid,
        /// <summary>
        /// Token comentário
        /// </summary>
        TokenComment
    }
}