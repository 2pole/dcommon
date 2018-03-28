using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.Formula.Functions;

namespace DCommon.LinqToNPOI.Writer
{
    public interface IExcelColumnBuilder<T> : INestedExcelColumnBuilder<T>, IRootExcelColumnBuilder<T>, IEnumerable<ExcelColumn<T>>
        where T : class
    {
        Int32 ColumnCount { get; }
    }
}
