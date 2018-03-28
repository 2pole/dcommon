using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DCommon.LinqToNPOI.Writer
{    
    public interface IRootExcelColumnBuilder<T>
        where T : class
    {       
        INestedExcelColumnBuilder<T> For(String name);
               
        INestedExcelColumnBuilder<T> For(Expression<Func<T, Object>> expression);

        INestedExcelColumnBuilder<T> For(Func<T, Object> func, String name);
    }
}
