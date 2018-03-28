using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NPOI.SS.UserModel;

namespace DCommon.LinqToNPOI.Writer
{  
    public interface INestedExcelColumnBuilder<T> where T : class
    {
        INestedExcelColumnBuilder<T> Formatted(String format);
               
        INestedExcelColumnBuilder<T> Href(String href);
             
        INestedExcelColumnBuilder<T> Href(Func<T, Object> expression);

        INestedExcelColumnBuilder<T> Header(Action<ICell> block);

        INestedExcelColumnBuilder<T> HeaderStyle(Action<ICellStyle> block);

        INestedExcelColumnBuilder<T> BodyStyle(Action<ICellStyle> block);

        INestedExcelColumnBuilder<T> Do(Action<T, ICell> block);
    }
}
