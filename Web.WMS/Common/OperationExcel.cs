using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Web.WMS.Common
{
    public class OperationExcel
    {
        /// <summary>
        /// 导出数据到Excel中(.xls)
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="Response"></param>
        /// <param name="fileName">文件名称</param>
        public void ExportExcel(DataTable dt, HttpResponseBase Response, string fileName)
        {
            try
            {
                //HSSFWorkbook workbook = new HSSFWorkbook();
                //MemoryStream ms = new MemoryStream();
                //// 新增sheet
                //HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");
                //// 插入标题
                //HSSFRow hRow = (HSSFRow)sheet.CreateRow(0);
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    hRow.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                //}
                //// 插入数据
                //for (int r = 0; r < dt.Rows.Count; r++)
                //{
                //    // 超过65535条数据则省略
                //    if (r >= 65535) break;
                //    hRow = (HSSFRow)sheet.CreateRow(r + 1);
                //    for (int i = 0; i < dt.Columns.Count; i++)
                //    {
                //        hRow.CreateCell(i).SetCellValue(dt.Rows[r][i].ToString());
                //    }
                //}
                //Excel工作表的sheet数目
                //int sheetNum = dt.Rows.Count % 65000 == 0 ? dt.Rows.Count / 65000 : (int)dt.Rows.Count / 65000 + 1;
                int sheetNum = dt.Rows.Count / 65001 + 1;

                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                for (int i = 1; i <= sheetNum; i++)//i是页数
                {
                    // 新增sheet
                    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet" + i.ToString());
                    // 插入标题
                    HSSFRow hRow = (HSSFRow)sheet.CreateRow(0);
                    HSSFCellStyle TitleStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFFont font = (HSSFFont)workbook.CreateFont();
                    TitleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;//细直线
                    TitleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    TitleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    TitleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    font.FontName = "宋体";
                    font.Boldweight = 600;//粗体
                    TitleStyle.SetFont(font);
                    TitleStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                    TitleStyle.FillPattern = FillPattern.BigSpots;
                    TitleStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                    TitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中
                    TitleStyle.VerticalAlignment = VerticalAlignment.Center;
                    for (int j = 0; j < dt.Columns.Count; j++)//j是插入标题的列数
                    {
                        ICell cell;
                        cell = hRow.CreateCell(j);
                        cell.SetCellValue(dt.Columns[j].ColumnName);
                        cell.CellStyle = TitleStyle;
                        //hRow.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                    }
                    // 插入数据
                    if (i != sheetNum)
                    {
                        for (int r = 0; r < 65000; r++)//r是数据条数
                        {
                            hRow = (HSSFRow)sheet.CreateRow(r + 1);
                            for (int k = 0; k < dt.Columns.Count; k++)
                            {
                                hRow.CreateCell(k).SetCellValue(dt.Rows[65000 * (i - 1) + r][k].ToString());
                            }
                        }
                    }
                    else
                    {
                        //最后一行
                        for (int r = 0; r < dt.Rows.Count - 65000 * (i - 1); r++)//r是数据条数
                        {
                            hRow = (HSSFRow)sheet.CreateRow(r + 1);
                            for (int k = 0; k < dt.Columns.Count; k++)
                            {
                                hRow.CreateCell(k).SetCellValue(dt.Rows[65000 * (i - 1) + r][k].ToString());
                            }
                        }
                    }
                }
                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileName));
                Response.BinaryWrite(ms.ToArray());

                workbook = null;
                ms.Close();
                ms.Dispose();
            }
            catch (Exception e)
            {
                Response.Write("<script type='text/javascript'>alert('" + e.Message + "');</script>");
            }
        }

        //public System.Data.DataTable ImportExcel(string fileName)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "data source=" + fileName + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
        //        OleDbConnection conn = new OleDbConnection(strConn);
        //        conn.Open();
        //        OleDbDataAdapter odda = new OleDbDataAdapter("select * from [Sheet1$]", conn);
        //        odda.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        #region Excel2003
        /// <summary>  
        /// 将Excel文件中的数据读出到DataTable中(xls)  
        /// </summary>  
        /// <param name="file"></param>  
        /// <returns></returns>  
        public DataTable ExcelToTableForXLS(string file)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                ISheet sheet = hssfworkbook.GetSheetAt(0);

                //表头  
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        //continue;  
                    }
                    else
                    {
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    }
                    columns.Add(i);
                }
                //数据  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        if (sheet.GetRow(i) == null)
                        {
                            break;
                        }
                        dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                fs.Close();
            }
            return dt;
        }

        /// <summary>  
        /// 将DataTable数据导出到Excel文件中(xls)  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <param name="file"></param>  
        public void TableToExcelForXLS(DataTable dt, string file)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Test");

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            hssfworkbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        /// <summary>  
        /// 获取单元格类型(xls)  
        /// </summary>  
        /// <param name="cell"></param>  
        /// <returns></returns>  
        private object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    //return cell.NumericCellValue;
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    else
                    {
                        // Numeric type
                        return cell.NumericCellValue;
                    }
                case CellType.String: //STRING:  
                    return cell.StringCellValue.ToString().Trim();
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }
        #endregion

        #region Excel2007
        /// <summary>  
        /// 将Excel文件中的数据读出到DataTable中(xlsx)  
        /// </summary>  
        /// <param name="file"></param>  
        /// <returns></returns>  
        public DataTable ExcelToTableForXLSX(string file)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                ISheet sheet = xssfworkbook.GetSheetAt(0);

                //表头  
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        //continue;  
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //数据  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        if (sheet.GetRow(i) == null)
                        {
                            break;
                        }
                        dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>  
        /// 将DataTable数据导出到Excel文件中(xlsx)  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <param name="file"></param>  
        public void TableToExcelForXLSX(DataTable dt, string file)
        {
            XSSFWorkbook xssfworkbook = new XSSFWorkbook();
            ISheet sheet = xssfworkbook.CreateSheet("Test");

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            xssfworkbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        /// <summary>  
        /// 获取单元格类型(xlsx)  
        /// </summary>  
        /// <param name="cell"></param>  
        /// <returns></returns>  
        private object GetValueTypeForXLSX(XSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    //return cell.NumericCellValue;
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    else
                    {
                        // Numeric type
                        return cell.NumericCellValue;
                    }
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }
        #endregion

        #region 读取xls中的数据
        public DataTable ImportExcel(string fileName)
        {
            DataTable table = new DataTable();
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                //获取excel的第一个sheet
                HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);
                //获取sheet的首行
                HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
                //一行最后一个方格的编号 即总的列数
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }
                //最后一列的标号  即总的行数
                int rowCount = sheet.LastRowNum;

                string[] formats = new string[] { "yyyy/MM/dd", "M/d/yy", "MM/d/yy", "M/dd/yy", "MM/dd/yy"};
                IFormatProvider ifp = new CultureInfo("zh-CN", true);
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    HSSFRow row = (HSSFRow)sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    // 一整条数据都为空则跳出
                    if (row == null)
                    {
                        break;
                    }
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            string value = row.GetCell(j).ToString();
                            DateTime dt;
                            if (DateTime.TryParseExact(value, formats, ifp, DateTimeStyles.NoCurrentDateDefault, out dt))
                            {
                                dataRow[j] = dt;
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                }
                workbook = null;
                sheet = null;
                return table;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}