﻿using Bite.Runtime.Bytecode;

namespace Bite.Runtime.Memory
{

public class FastClassMemorySpace : FastMemorySpace
{
    #region Public

    public FastClassMemorySpace(
        string name,
        FastMemorySpace enclosingSpace,
        int stackCount,
        BinaryChunk callerChunk,
        int callerInstructionPointer,
        int memberCount ) : base( $"$class_{name}", enclosingSpace, stackCount, callerChunk, callerInstructionPointer, memberCount )
    {
    }

    #endregion
}

}
