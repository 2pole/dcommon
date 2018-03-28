using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace DCommon.LinqToNPOI.Writer
{
    public class ExcelExporter : IRootExcelBuilder, IExcelExporter
    {
        #region Public Properties
        public Action<DocumentSummaryInformation> DocumentProperty { get; set; }
       
        public Action<SummaryInformation> SummaryProperty { get; set; }

        public Action<ICellStyle> DefaultHeaderStyle { get; set; }

        public Action<ICellStyle> DefaultBodyStyle { get; set; }

        public ICellStyle HeaderDefaultStyle { get; set; }

        public ICellStyle BodyDefaultStyle { get; set; }

        public IWorkbook Workbook { get; set; } 
        #endregion       

        #region Constructor
        public ExcelExporter()
        {
            this.Workbook = new HSSFWorkbook();
            this.Initialize();
        }

        private void Initialize()
        {
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();

            // -----------------------------------------------
            //  SummaryInformation
            // -----------------------------------------------
            si.Author = "LinqToNPOI";
            si.CreateDateTime = DateTime.Now;
            si.LastSaveDateTime = DateTime.Now;
            if (this.DocumentProperty != null)
            {
                this.DocumentProperty(dsi);
            }
            if (this.SummaryProperty != null)
            {
                this.SummaryProperty(si);
            }
            var workbook = this.Workbook as HSSFWorkbook;
            if (workbook != null)
            {
                workbook.SummaryInformation = si;
                workbook.DocumentSummaryInformation = dsi;
            }

            var headerStyle = this.Workbook.CreateCellStyle();
            this.SetDefaultHeaderStyle(headerStyle);
            if (this.DefaultHeaderStyle !=  null)
            {
                this.DefaultHeaderStyle(headerStyle);
            }
            this.HeaderDefaultStyle = headerStyle;

            var bodyStyle = this.Workbook.CreateCellStyle();
            this.SetDefaultBodyStyle(bodyStyle);
            if (this.DefaultBodyStyle != null)
            {
                this.DefaultBodyStyle(bodyStyle);
            }
            this.BodyDefaultStyle = bodyStyle;
        }

        #endregion

        #region IExcelExporter

        /// <summary>
        /// Save excel.
        /// </summary>
        public void Save(Stream streamWriter)
        {
            this.Workbook.Write(streamWriter);
        }
        #endregion

        #region IRootExcelSheetBuilder Members
        public ISheet CurrentSheet
        {
            get;
            private set;
        }

        public void GenerateSheet<T>(String sheetName, IEnumerable<T> items, IExcelColumnBuilder<T> columns)
            where T : class
        {
            ISheet sheet = String.IsNullOrEmpty(sheetName) ? this.Workbook.CreateSheet() : this.Workbook.CreateSheet(sheetName);
            this.CurrentSheet = sheet;
            this.GenerateHeader(columns, sheet);
            this.GenerateItems(items, columns, sheet);
            this.AutoSizeColumns(columns.ColumnCount);
        }

        private void AutoSizeColumns(int columnCount)
        {
            int current = 0;
            if (CurrentSheet != null)
            {
                while (current < columnCount)
                {
                    CurrentSheet.AutoSizeColumn(current, true);
                    current++;
                }
            }
        }

        #endregion

        #region Private Methods

        protected virtual void GenerateItems<T>(IEnumerable<T> dataSource, IExcelColumnBuilder<T> columns, ISheet sheet)
            where T : class
        {           
            if (!dataSource.Any())
            {
                return;
            }

            List<ICellStyle> bodyStyles = new List<ICellStyle>(columns.ColumnCount);

            foreach (var column in columns)
            {
                #region Setting Styles
                var bodyStyle = this.Workbook.CreateCellStyle();
                this.SetDefaultBodyStyle(bodyStyle);
                if (column.BodyStyle != null)
                {
                    column.BodyStyle(bodyStyle);
                }              
                bodyStyles.Add(bodyStyle);
                #endregion
            }
           
            //Head at frist row.
            Int32 rowIndex = 1;
            foreach (var item in dataSource)
            {
                IRow row = sheet.CreateRow(rowIndex);
                Int32 columnIndex = 0;
                rowIndex++;

                foreach (var column in columns)
                {                   
                    var cell = row.CreateCell(columnIndex);
                    cell.CellStyle = bodyStyles[columnIndex];                    
                    if (column.CustomRenderer == null)
                    {
                        #region Fetch t& formated the data.
                        Object value = null;
                        if (column.ColumnDelegate != null)
                        {
                            value = column.ColumnDelegate(item);
                        }
                        else
                        {
                            var property = item.GetType().GetProperty(column.Name);
                            if (property != null)
                            {
                                value = property.GetValue(item, null);
                            }
                        }

                        String formattedValue = null;
                        if (value != null)
                        {
                            if (column.Format != null)
                            {
                                formattedValue = String.Format(column.Format, value);
                            }
                            else
                            {
                                formattedValue = value.ToString();
                            }
                        }
                        #endregion

                        cell.SetCellValue(formattedValue);
                    }
                    else
                    {                       
                        column.CustomRenderer(item, cell);                        
                    }

                    #region Setting href
                    if (null != column.HrefDelegate)
                    {
                        var href = column.HrefDelegate(item);
                        if (null != href)
                        {
                            var link = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = href.ToString()
                            };
                            cell.Hyperlink = link;
                        }
                    }
                    #endregion

                    columnIndex++;
                }
            }
        }

        protected virtual void GenerateHeader<T>(IExcelColumnBuilder<T> columns, ISheet sheet)
            where T : class
        {
            sheet.DefaultColumnWidth = 20;
            sheet.DefaultRowHeight = 14;

            var headRow = sheet.CreateRow(0);
            Int32 columnIndex = 0;               
            foreach (var column in columns)
            {               
                var headerStyle = this.Workbook.CreateCellStyle();
                var columnCell = headRow.CreateCell(columnIndex);
                this.SetDefaultHeaderStyle(headerStyle);                
                if (column.HeaderStyle != null)
                {                    
                    column.HeaderStyle(headerStyle);
                }                
                if (column.CustomHeader != null)
                {           
                    column.CustomHeader(columnCell);
                }
                else
                {   
                    columnCell.SetCellValue(column.Name);
                }               
                columnCell.CellStyle = headerStyle;

                columnIndex++;
            }          
            headRow.CreateCell(columnIndex);
        }

        private void SetDefaultHeaderStyle(ICellStyle style)
        {
            style.Alignment = HorizontalAlignment.Center;
            IFont font = this.Workbook.CreateFont();              
            font.FontHeightInPoints = 12;
            style.SetFont(font);
        }

        private void SetDefaultBodyStyle(ICellStyle style)
        {
            IFont font = this.Workbook.CreateFont();            
            font.FontHeightInPoints = 11;
            style.SetFont(font);
        }

        #endregion
    }
}
