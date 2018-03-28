using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DCommon.Search.Expressions
{
    internal class ExpressionConstants
    {
        // Properties
        internal static Expression FalseLiteral
        {
            get
            {
                return Expression.Constant(false);
            }
        }

        internal static Expression NullLiteral
        {
            get
            {
                return Expression.Constant(null);
            }
        }

        internal static Expression TrueLiteral
        {
            get
            {
                return Expression.Constant(true);
            }
        }
    }
}
