using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.LinqToNPOI.Writer;
using NPOI.HPSF;
using NPOI.SS.UserModel;

namespace DCommon.LinqToNPOI
{
    public static class ExcelBuilder
    {
        public static IRootExcelBuilder Create()
        {
            return new ExcelExporter();
        }

        public static IRootExcelBuilder SetDocument(this IRootExcelBuilder exporter, Action<DocumentSummaryInformation> documentProperty)
        {
            exporter.DocumentProperty = documentProperty;
            return exporter;
        }

        public static IRootExcelBuilder SetSummary(this IRootExcelBuilder exporter, Action<SummaryInformation> summaryProperty)
        {
            exporter.SummaryProperty = summaryProperty;
            return exporter;
        }

        public static IRootExcelBuilder SetDefaultHeaderStyle(this IRootExcelBuilder exporter, Action<ICellStyle> defaultHeaderStyle)
        {
            exporter.DefaultHeaderStyle = defaultHeaderStyle;
            return exporter;
        }

        public static IRootExcelBuilder SetDefaultBodyStyle(this IRootExcelBuilder exporter, Action<ICellStyle> defaultBodyStyle)
        {
            exporter.DefaultBodyStyle = defaultBodyStyle;
            return exporter;
        }

        public static IExcelExporter Sheet<T>(this IExcelExporter reporter, string sheetName, IEnumerable<T> dataSource, Action<IRootExcelColumnBuilder<T>> columns)
            where T : class
        {
            var columnBuilder = CreateColumnBuilder(columns);
            reporter.GenerateSheet(sheetName, dataSource, columnBuilder);
            return reporter;
        }

        public static IExcelExporter Sheet<T>(this IExcelExporter reporter, string sheetName, IEnumerable<T> dataSource, IExcelColumnBuilder<T> columns)
            where T : class
        {
            reporter.GenerateSheet(sheetName, dataSource, columns);
            return reporter;
        }

        private static IExcelColumnBuilder<T> CreateColumnBuilder<T>(Action<IRootExcelColumnBuilder<T>> columns)
           where T : class
        {
            var builder = new ExcelColumnBuilder<T>();

            if (columns != null)
            {
                columns(builder);
            }

            return builder;
        }

        public static IExcelExporter ToExcel(this Action<IRootExcelBuilder> sheets)
        {
            var exporter = new ExcelExporter();
            if (null != sheets)
            {
                sheets(exporter);
            }
            return exporter;
        }

        public static IExcelExporter ToExcel<T>(this IEnumerable<T> dataSource, string sheetName, Action<IRootExcelColumnBuilder<T>> columns)
            where T : class
        {
            return Create().Sheet(sheetName, dataSource, columns);
        }
    }
}
