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
        /// Token dois pontos (:)
        /// </summary>
        TokenColon,
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
        TokenCADivision,
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
        TokenComment,
        /// <summary>
        /// Token palavra reservada "async"
        /// </summary>
        TokenKeywordAsync,
        /// <summary>
        /// Token palavra reservada "await"
        /// </summary>
        TokenKeywordAwait,
        /// <summary>
        /// Token palavra reservada "assert"
        /// </summary>
        TokenKeywordAssert,
        /// <summary>
        /// Token palavra reservada "break"
        /// </summary>
        TokenKeywordBreak,
        /// <summary>
        /// Token palavra reservada "case"
        /// </summary>
        TokenKeywordCase,
        /// <summary>
        /// Token palavra reservada "catch"
        /// </summary>
        TokenKeywordCatch,
        /// <summary>
        /// Token palavra reservada "class"
        /// </summary>
        TokenKeywordClass,
        /// <summary>
        /// Token palavra reservada "const"
        /// </summary>
        TokenKeywordConst,
        /// <summary>
        /// Token palavra reservada "continue"
        /// </summary>
        TokenKeywordContinue,
        /// <summary>
        /// Token palavra reservada "default"
        /// </summary>
        TokenKeywordDefault,
        /// <summary>
        /// Token palavra reservada "do"
        /// </summary>
        TokenKeywordDo,
        /// <summary>
        /// Token palavra reservada "else"
        /// </summary>
        TokenKeywordElse,
        /// <summary>
        /// Token palavra reservada "enum"
        /// </summary>
        TokenKeywordEnum,
        /// <summary>
        /// Token palavra reservada "static"
        /// </summary>
        TokenKeywordStatic,
                /// <summary>
        /// Token palavra reservada "get"
        /// </summary>
        TokenKeywordGet,
                /// <summary>
        /// Token palavra reservada "set"
        /// </summary>
        TokenKeywordSet,
        /// <summary>
        /// Token palavra reservada "extends"
        /// </summary>
        TokenKeywordExtends,
        /// <summary>
        /// Token palavra reservada "implements"
        /// </summary>
        TokenKeywordImplements,
        /// <summary>
        /// Token palavra reservada "false"
        /// </summary>
        TokenKeywordFalse,
        /// <summary>
        /// Token palavra reservada "final"
        /// </summary>
        TokenKeywordFinal,
        /// <summary>
        /// Token palavra reservada "finally"
        /// </summary>
        TokenKeywordFinally,
        /// <summary>
        /// Token palavra reservada "for"
        /// </summary>
        TokenKeywordFor,
        /// <summary>
        /// Token palavra reservada "if"
        /// </summary>
        TokenKeywordIf,
        /// <summary>
        /// Token palavra reservada "in"
        /// </summary>
        TokenKeywordIn,
        /// <summary>
        /// Token palavra reservada "is"
        /// </summary>
        TokenKeywordIs,
        /// <summary>
        /// Token palavra reservada "new"
        /// </summary>
        TokenKeywordNew,
        /// <summary>
        /// Token palavra reservada "null"
        /// </summary>
        TokenKeywordNull,
        /// <summary>
        /// Token palavra reservada "rethrow"
        /// </summary>
        TokenKeywordRethrow,
        /// <summary>
        /// Token palavra reservada "return"
        /// </summary>
        TokenKeywordReturn,
        /// <summary>
        /// Token palavra reservada "super"
        /// </summary>
        TokenKeywordSuper,
        /// <summary>
        /// Token palavra reservada "switch"
        /// </summary>
        TokenKeywordSwitch,
        /// <summary>
        /// Token palavra reservada "this"
        /// </summary>
        TokenKeywordThis,
        /// <summary>
        /// Token palavra reservada "throw"
        /// </summary>
        TokenKeywordThrow,
        /// <summary>
        /// Token palavra reservada "true"
        /// </summary>
        TokenKeywordTrue,
        /// <summary>
        /// Token palavra reservada "try"
        /// </summary>
        TokenKeywordTry,
        /// <summary>
        /// Token palavra reservada "var"
        /// </summary>
        TokenKeywordVar,
        /// <summary>
        /// Token palavra reservada "void"
        /// </summary>
        TokenKeywordVoid,
        /// <summary>
        /// Token palavra reservada "while"
        /// </summary>
        TokenKeywordWhile,
        /// <summary>
        /// Token palavra reservada "with"
        /// </summary>
        TokenKeywordWith,
        /// <summary>
        /// Token palavra reservada "dynamic"
        /// </summary>
        TokenKeywordDynamic,
        /// <summary>
        /// Token palavra reservada "import"
        /// </summary>
        TokenKeywordImport
    }
}