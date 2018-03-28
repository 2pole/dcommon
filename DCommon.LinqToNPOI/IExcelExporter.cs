using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DCommon.LinqToNPOI.Writer;

namespace DCommon.LinqToNPOI
{
    public interface IExcelExporter
    {
        void GenerateSheet<T>(String sheetName, IEnumerable<T> dataSource, IExcelColumnBuilder<T> columns) where T : class;

        void Save(Stream writer);
    }
}
