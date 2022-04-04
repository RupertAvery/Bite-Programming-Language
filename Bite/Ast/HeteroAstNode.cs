using Bite.SymbolTable;

namespace Bite.Ast
{

    public abstract class HeteroAstNode 
    {
        public Scope AstScopeNode { get; set; }

        public DebugInfo DebugInfoAstNode = new DebugInfo();

        #region Public

        public abstract object Accept(IAstVisitor visitor);

        #endregion
    }

}
