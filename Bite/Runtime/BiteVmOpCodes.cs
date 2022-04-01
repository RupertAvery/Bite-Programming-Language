﻿namespace Bite.Runtime
{

    public enum SrslVmOpCodes : byte
    {
        OpNone,
        OpPopStack,
        OpConstant,
        OpAssign,
        OpPlusAssign,
        OpMinusAssign,
        OpMultiplyAssign,
        OpDivideAssign,
        OpModuloAssign,
        OpBitwiseAndAssign,
        OpBitwiseOrAssign,
        OpBitwiseXorAssign,
        OpBitwiseLeftShiftAssign,
        OpBitwiseRightShiftAssign,
        OpAdd,
        OpSubtract,
        OpMultiply,
        OpDivide,
        OpModulo,
        OpEqual,
        OpNotEqual,
        OpSmaller,
        OpSmallerEqual,
        OpGreater,
        OpGreaterEqual,
        OpAnd,
        OpOr,
        OpPostfixIncrement,
        OpPostfixDecrement,
        OpPrefixIncrement,
        OpPrefixDecrement,
        OpNegate,
        OpAffirm,
        OpCompliment,
        OpNot,
        OpDefineModule,
        OpDefineClass,
        OpUsingStatmentHead,
        OpUsingStatmentEnd,
        OpDefineMethod,
        OpDefineLocalVar,
        OpDeclareLocalVar,
        OpSetLocalVar,
        OpGetLocalVar,
        OpGetModule,
        OpDefineLocalInstance,
        OpSetLocalInstance,
        OpGetLocalInstance,
        OpJumpIfFalse,
        OpJump,
        OpEnterBlock,
        OpExitBlock,
        OpForLoopHeader,
        OpForLoopBody,
        OpWhileLoop,
        OpBindToFunction,
        OpGetElement,
        OpSetElement,
        OpCallFunction,
        OpCallMemberFunction,
        OpGetMember,
        OpSetMember,
        OpImportModule,
        OpKeepLastItemOnStack,
        OpBreak,
        OpReturn
    }

}
