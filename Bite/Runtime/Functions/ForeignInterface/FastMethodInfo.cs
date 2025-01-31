﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Bite.Runtime.Functions.ForeignInterface
{

public class FastMethodInfo
{
    private delegate object ReturnValueDelegate( object instance, object[] arguments );

    private delegate void VoidDelegate( object instance, object[] arguments );

    private ReturnValueDelegate Delegate { get; }

    #region Public

    public FastMethodInfo( MethodInfo methodInfo )
    {
        ParameterExpression instanceExpression = Expression.Parameter( typeof( object ), "instance" );
        ParameterExpression argumentsExpression = Expression.Parameter( typeof( object[] ), "arguments" );
        List < Expression > argumentExpressions = new List < Expression >();
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();

        for ( int i = 0; i < parameterInfos.Length; ++i )
        {
            ParameterInfo parameterInfo = parameterInfos[i];

            argumentExpressions.Add(
                Expression.Convert(
                    Expression.ArrayIndex( argumentsExpression, Expression.Constant( i ) ),
                    parameterInfo.ParameterType ) );
        }

        MethodCallExpression callExpression = Expression.Call(
            !methodInfo.IsStatic ? Expression.Convert( instanceExpression, methodInfo.ReflectedType ) : null,
            methodInfo,
            argumentExpressions );

        if ( callExpression.Type == typeof( void ) )
        {
            VoidDelegate voidDelegate = Expression.Lambda < VoidDelegate >(
                                                       callExpression,
                                                       instanceExpression,
                                                       argumentsExpression ).
                                                   Compile();

            Delegate = ( instance, arguments ) =>
            {
                voidDelegate( instance, arguments );

                return null;
            };
        }
        else
        {
            Delegate = Expression.Lambda < ReturnValueDelegate >(
                                      Expression.Convert( callExpression, typeof( object ) ),
                                      instanceExpression,
                                      argumentsExpression ).
                                  Compile();
        }
    }

    public object Invoke( object instance, params object[] arguments )
    {
        return Delegate( instance, arguments );
    }

    #endregion
}

}
