namespace Bite.Ast
{

    public class LocalVariableDeclarationNode : StatementNode
    {
        public ModifiersNode Modifiers;
        public Identifier VarId;
        public ExpressionNode Expression;
        public override object Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ForInitializerNode : HeteroAstNode
    {
        public LocalVariableDeclarationNode VariableDeclaration { get; set; }
        public ExpressionNode[] Expressions { get; set; }

        public override object Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class ForStatementNode : StatementNode
    {
        public ForInitializerNode Initializer { get; set; }
        public ExpressionNode Condition { get; set; }
        public ExpressionNode[] Iterators { get; set; }
        public StatementNode Statement { get; set; }


        // TODO: Remove
        public VariableDeclarationNode VariableDeclaration;
        public ExpressionNode Iterator;
        public ExpressionStatementNode ExpressionStatement;
        public BlockStatementNode Block;
        #region Public

        public override object Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

        #endregion
    }

}
