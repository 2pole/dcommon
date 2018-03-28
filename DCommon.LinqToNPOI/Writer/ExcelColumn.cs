using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace DCommon.LinqToNPOI.Writer
{    
    public class ExcelColumn<T>
    {        
        public String Name { get; set; }

        public Func<T, Object> HrefDelegate { get; set; }

        public Action<ICellStyle> HeaderStyle { get; set; }

        public Action<ICellStyle> BodyStyle { get; set; }
               
        public String Format { get; set; }

        public Action<ICell> CustomHeader { get; set; }

        public Func<T, Object> ColumnDelegate { get; set; }

        public Action<T, ICell> CustomRenderer { get; set; }
    }
}
