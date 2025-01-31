using System.Collections.Generic;

namespace Bite.Ast
{

public class ModifiersNode : HeteroAstNode
{
    public enum ModifierTypes
    {
        DeclarePrivate,
        DeclarePublic,
        DeclareAbstract,
        DeclareStatic
    }

    public List < ModifierTypes > Modifiers;

    #region Public

    public ModifiersNode( string accessMod, string staticAbstractMod )
    {
        Modifiers = new List < ModifierTypes >();

        if ( accessMod != null && accessMod == "public" )
        {
            Modifiers.Add( ModifierTypes.DeclarePublic );
        }
        else
        {
            Modifiers.Add( ModifierTypes.DeclarePrivate );
        }

        if ( staticAbstractMod != null && staticAbstractMod == "static")
        {
            Modifiers.Add( ModifierTypes.DeclareStatic );
        }
        else if ( staticAbstractMod != null && staticAbstractMod == "abstract" )
        {
            Modifiers.Add( ModifierTypes.DeclareAbstract );
        }
    }

    public override object Accept( IAstVisitor visitor )
    {
        return visitor.Visit( this );
    }

    #endregion
}

}
