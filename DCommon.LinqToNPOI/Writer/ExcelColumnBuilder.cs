using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NPOI.SS.UserModel;

namespace DCommon.LinqToNPOI.Writer
{
    public class ExcelColumnBuilder<T> : IExcelColumnBuilder<T>
        where T : class
    {
        private readonly List<ExcelColumn<T>> columns = new List<ExcelColumn<T>>();

        private ExcelColumn<T> currentColumn;

        public ExcelColumn<T> this[int index]
        {
            get
            {
                return columns[index];
            }
        }
      
        public Int32 ColumnCount
        {
            get { return columns.Count; }
        }

        #region Constructor
        public ExcelColumnBuilder()
        {
        }
        #endregion
       
        public static String ExpressionToName(Expression<Func<T, Object>> expression)
        {
            var memberExpression = RemoveUnary(expression.Body) as MemberExpression;

            return memberExpression == null ? String.Empty : memberExpression.Member.Name;
        }

        private static Expression RemoveUnary(Expression body)
        {
            var unary = body as UnaryExpression;
            if (unary != null)
            {
                return unary.Operand;
            }
            return body;
        }

        #region IRootExcelColumnBuilder<T> Members
            
        public INestedExcelColumnBuilder<T> For(String name)
        {
            currentColumn = new ExcelColumn<T> { Name = name };
            columns.Add(currentColumn);
            return this;
        }

        public INestedExcelColumnBuilder<T> For(Expression<Func<T, Object>> expression)
        {
            currentColumn = new ExcelColumn<T>
            {
                Name = ExpressionToName(expression),
                ColumnDelegate = expression.Compile(),
            };

            columns.Add(currentColumn);
            return this;
        }
             
        public INestedExcelColumnBuilder<T> For(Func<T, Object> func, String name)
        {
            currentColumn = new ExcelColumn<T>
            {
                Name = name,
                ColumnDelegate = func
            };
            columns.Add(currentColumn);
            return this;
        }
        #endregion

        #region INestedExcelColumnBuilder<T> Members

        public INestedExcelColumnBuilder<T> Formatted(String format)
        {
            this.currentColumn.Format = format;
            return this;
        }

        public INestedExcelColumnBuilder<T> Href(string href)
        {
            this.currentColumn.HrefDelegate = (e => { return href; });
            return this;
        }

        public INestedExcelColumnBuilder<T> Href(Func<T, Object> expression)
        {
            this.currentColumn.HrefDelegate = expression;
            return this;
        }
           
        public INestedExcelColumnBuilder<T> Do(Action<T, ICell> block)
        {
            currentColumn.CustomRenderer = block;
            return this;
        }

        public INestedExcelColumnBuilder<T> Header(Action<ICell> block)
        {
            currentColumn.CustomHeader = block;
            return this;
        }

        public INestedExcelColumnBuilder<T> HeaderStyle(Action<ICellStyle> block)
        {
            currentColumn.HeaderStyle = block;
            return this;
        }

        public INestedExcelColumnBuilder<T> BodyStyle(Action<ICellStyle> block)
        {
            currentColumn.BodyStyle = block;
            return this;
        }

        #endregion

        #region IEnumerable<ExcelColumn<T>> Members

        public IEnumerator<ExcelColumn<T>> GetEnumerator()
        {
            return this.columns.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.columns.GetEnumerator();
        }

        #endregion
    }
}
