using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HPSF;
using NPOI.SS.UserModel;

namespace DCommon.LinqToNPOI.Writer
{    
    public interface IRootExcelBuilder : IExcelExporter
    {       
        Action<DocumentSummaryInformation> DocumentProperty { get; set; }

        Action<SummaryInformation> SummaryProperty { get; set; }

        Action<ICellStyle> DefaultHeaderStyle { get; set; }

        Action<ICellStyle> DefaultBodyStyle { get; set; }

        IWorkbook Workbook { get;  }

        ISheet CurrentSheet { get; }
    }
}
