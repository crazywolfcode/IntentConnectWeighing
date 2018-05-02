using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using System.Windows.Controls;

namespace MyHelper
{
    public class ExclHelper
    {


        #region  wpf客户端 导出DataGrid数据到Excel

        public static void Export(System.Windows.Controls.DataGrid dataGrid, string excelTitle)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visibility == System.Windows.Visibility.Visible)//只导出可见列  
                {
                    dt.Columns.Add(dataGrid.Columns[i].Header.ToString());//构建表头  
                }
            }

            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                int columnsIndex = 0;
                System.Data.DataRow row = dt.NewRow();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    if (dataGrid.Columns[j].Visibility == System.Windows.Visibility.Visible)
                    {
                        if (dataGrid.Items[i] != null && (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock) != null)//填充可见列数据  
                        {
                            row[columnsIndex] = (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock).Text.ToString();
                        }
                        else
                        {
                            row[columnsIndex] = "";
                        }
                        columnsIndex++;
                    }
                }
                dt.Rows.Add(row);
            }

            string FileName = ExcelExport(dt, excelTitle);
        }
        public static string ExcelExport(System.Data.DataTable DT, string title)
        {
            try
            {
                //创建Excel
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook ExcelBook = ExcelApp.Workbooks.Add(System.Type.Missing);
                //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出
                Microsoft.Office.Interop.Excel.Worksheet ExcelSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelBook.Worksheets[1];

                //如果数据中存在数字类型 可以让它变文本格式显示
                ExcelSheet.Cells.NumberFormat = "@";

                //设置工作表名
                ExcelSheet.Name = title;

                //设置Sheet标题
                string start = "A1";
                string end = ChangeASC(DT.Columns.Count) + "1";

                Microsoft.Office.Interop.Excel.Range _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);
                _Range.Merge(0);                     //单元格合并动作(要配合上面的get_Range()进行设计)
                _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);
                _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                _Range.Font.Size = 18; //设置字体大小
                _Range.Font.Name = "黑体"; //设置字体的种类 
                ExcelSheet.Cells[1, 1] = title;    //Excel单元格赋值
                _Range.EntireColumn.AutoFit(); //自动调整列宽

                //写表头
                for (int m = 0; m < DT.Columns.Count; m++)
                {
                    ExcelSheet.Cells[2, m + 1] = DT.Columns[m].ColumnName.ToString();

                    start = "A2";
                    end = ChangeASC(DT.Columns.Count) + "2";

                    _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);
                    _Range.Font.Size = 13; //设置字体大小
                    _Range.Font.Name = "黑体"; //设置字体的种类  
                    _Range.EntireColumn.AutoFit(); //自动调整列宽 
                    _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }

                //写数据
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    for (int j = 0; j < DT.Columns.Count; j++)
                    {
                        //Excel单元格第一个从索引1开始
                        // if (j == 0) j = 1;
                        ExcelSheet.Cells[i + 3, j + 1] = DT.Rows[i][j].ToString();
                    }
                }

                //表格属性设置
                for (int n = 0; n < DT.Rows.Count + 1; n++)
                {
                    start = "A" + (n + 3).ToString();
                    end = ChangeASC(DT.Columns.Count) + (n + 3).ToString();

                    //获取Excel多个单元格区域
                    _Range = (Microsoft.Office.Interop.Excel.Range)ExcelSheet.get_Range(start, end);

                    _Range.Font.Size = 12; //设置字体大小
                    _Range.Font.Name = "宋体"; //设置字体的种类

                    _Range.EntireColumn.AutoFit(); //自动调整列宽
                    _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter; //设置字体在单元格内的对其方式 _Range.EntireColumn.AutoFit(); //自动调整列宽 
                }

                ExcelApp.DisplayAlerts = false; //保存Excel的时候，不弹出是否保存的窗口直接进行保存 

                //弹出保存对话框,并保存文件
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.DefaultExt = ".xlsx";
                sfd.FileName = title;
                sfd.Filter = "Office 2007 File|*.xlsx|Office 2000-2003 File|*.xls|所有文件|WPS(*.et)|*.e|*.*";
                if (sfd.ShowDialog() == true)
                {
                    if (sfd.FileName != "")
                    {
                        ExcelBook.SaveAs(sfd.FileName);  //将其进行保存到指定的路径
                                                         // GlobalVar.ShowMsgInfo("导出文件存储为: " + sfd.FileName);
                    }
                }

                //释放可能还没释放的进程
                ExcelBook.Close();
                ExcelApp.Quit();
                //  PubHelper.Instance.KillAllExcel(ExcelApp);

                return sfd.FileName;
            }
            catch (Exception e)
            {
                //GlobalVar.ShowMsgWarning("导出文件保存失败！");
                MessageBox.Show("导出文件保存失败！" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取当前列列名,并得到EXCEL中对应的列
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string ChangeASC(int count)
        {
            string ascstr = "";

            switch (count)
            {
                case 1:
                    ascstr = "A";
                    break;
                case 2:
                    ascstr = "B";
                    break;
                case 3:
                    ascstr = "C";
                    break;
                case 4:
                    ascstr = "D";
                    break;
                case 5:
                    ascstr = "E";
                    break;
                case 6:
                    ascstr = "F";
                    break;
                case 7:
                    ascstr = "G";
                    break;
                case 8:
                    ascstr = "H";
                    break;
                case 9:
                    ascstr = "I";
                    break;
                case 10:
                    ascstr = "J";
                    break;
                case 11:
                    ascstr = "K";
                    break;
                case 12:
                    ascstr = "L";
                    break;
                case 13:
                    ascstr = "M";
                    break;
                case 14:
                    ascstr = "N";
                    break;
                case 15:
                    ascstr = "O";
                    break;
                case 16:
                    ascstr = "P";
                    break;
                case 17:
                    ascstr = "Q";
                    break;
                case 18:
                    ascstr = "R";
                    break;
                case 19:
                    ascstr = "S";
                    break;
                case 20:
                    ascstr = "Y";
                    break;
                default:
                    ascstr = "U";
                    break;
            }
            return ascstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="fileName"></param>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <param name="info"></param>
        /// <param name="summaryStr"></param>
        /// <param name="listSt"></param>
        /// <returns>-1 要导出的数据为空！0导出失败 1导出成功</returns>
        public static int ExclExprotToExcelWitchStatisticInfo(System.Windows.Controls.DataGrid dataGrid, String fileName, String title, String subTitle, String info, String summaryStr, List<String> listSt)
        {
            System.Data.DataTable dataTable = DataGridToDataTable(dataGrid);
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                return -1;
            }
            try
            {
                string file = string.Empty;
                //创建Excel
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook ExcelBook = ExcelApp.Workbooks.Add(System.Type.Missing);
                //创建工作表（即Excel里的子表sheet） 1表示在子表sheet1里进行数据导出
                Worksheet ExcelSheet = (Worksheet)ExcelBook.Worksheets[1];
                //如果数据中存在数字类型 可以让它变文本格式显示
                ExcelSheet.Cells.NumberFormat = "@";
                int a= 1;
                //设置工作表名
                ExcelSheet.Name = title;
                //设置Sheet标题
                string start = "A"+a;
                string end = ChangeASC(dataTable.Columns.Count) + a;
                Range _Range = ExcelSheet.get_Range(start, end);
                _Range.Merge(0);                     //单元格合并动作(要配合上面的get_Range()进行设计)
                _Range = ExcelSheet.get_Range(start, end);
                _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                _Range.Font.Size = 18; //设置字体大小
                _Range.Font.Name = "黑体"; //设置字体的种类 
                ExcelSheet.Cells[1, 1] = title;    //Excel单元格赋值
                _Range.EntireColumn.AutoFit(); //自动调整列宽

                //设置 二级标题
                if (!string.IsNullOrEmpty(subTitle))
                {
                    a++;
                    start = "A"+a;
                    end = ChangeASC(dataTable.Columns.Count) + a;
                    Microsoft.Office.Interop.Excel.Range range0 = ExcelSheet.get_Range(start, end);
                    range0.Merge(0);                     //单元格合并动作(要配合上面的get_Range()进行设计)
                    range0 = ExcelSheet.get_Range(start, end);
                    range0.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    range0.Font.Size = 12; //设置字体大小
                    range0.Font.Name = "黑体"; //设置字体的种类 
                    ExcelSheet.Cells[a, 1] = subTitle; //Excel单元格赋值
                    range0.EntireColumn.AutoFit(); //自动调整列宽
                }
                //设置数据信息
                if (!string.IsNullOrEmpty(info))
                {
                    a++;
                    start = "A" + a;
                    end = ChangeASC(dataTable.Columns.Count) + a;
                    Range range0 = ExcelSheet.get_Range(start, end);
                    range0.Merge(0);          
                    range0 = ExcelSheet.get_Range(start, end);
                    range0.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    range0.Font.Size = 12; //设置字体大小
                    range0.Font.Name = "宋体"; //设置字体的种类 
                    ExcelSheet.Cells[a, 1] = info; //Excel单元格赋值
                    range0.EntireColumn.AutoFit(); //自动调整列宽
                }

                //写表头
                a++;
                for (int m = 0; m < dataTable.Columns.Count; m++)
                {
                    ExcelSheet.Cells[a, m + 1] = dataTable.Columns[m].ColumnName.ToString();
                    start = "A"+a;
                    end = ChangeASC(dataTable.Columns.Count) + a;
                    _Range = ExcelSheet.get_Range(start, end);
                    _Range.Font.Size = 12; //设置字体大小
                    _Range.Font.Name = "黑体"; //设置字体的种类  
                    _Range.EntireColumn.AutoFit(); //自动调整列宽 
                    _Range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                }                
                //表格属性设置
                for (int n = 0; n < dataTable.Rows.Count + 1; n++)
                {
                    start = "A" +a+1;
                    end = ChangeASC(dataTable.Columns.Count) +a+1;
                    //获取Excel多个单元格区域
                    _Range = ExcelSheet.get_Range(start, end);
                    _Range.Font.Size = 12; //设置字体大小
                    _Range.Font.Name = "宋体"; //设置字体的种类
                    _Range.EntireColumn.AutoFit(); //自动调整列宽
                    //设置字体在单元格内的对其方式 _Range.EntireColumn.AutoFit(); //自动调整列宽 
                    _Range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                }
                //写数据
                a++;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        //Excel单元格第一个从索引1开始                   
                        ExcelSheet.Cells[i + a, j + 1] = dataTable.Rows[i][j].ToString();
                    }
                }
                // 写汇总统计信息
                if (!String.IsNullOrEmpty(summaryStr))
                {
                    a++;
                    start = "A" + (dataTable.Rows.Count + a);
                    end = ChangeASC(dataTable.Columns.Count) + (dataTable.Rows.Count + a);
                    Microsoft.Office.Interop.Excel.Range range = ExcelSheet.get_Range(start, end);
                    range.Merge(0);
                    range = ExcelSheet.get_Range(start, end);
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    range.EntireColumn.AutoFit(); //自动调整列宽
                    range.Font.Size = 12; //设置字体大小
                    range.Font.Name = "黑体"; //设置字体的种类 
                    ExcelSheet.Cells[dataTable.Rows.Count + a, 1] = summaryStr;
                }
                // 写列表统计信息
                if (listSt != null && listSt.Count > 0)
                {         
                    for (int i = 0; i < listSt.Count; i++)
                    {
                        a++;
                        start = "A" + (dataTable.Rows.Count + a);
                        end = ChangeASC(dataTable.Columns.Count) + (dataTable.Rows.Count + a);
                        Range rangetwo = ExcelSheet.get_Range(start, end);
                        rangetwo.Merge(0);
                        rangetwo = ExcelSheet.get_Range(start, end);
                        rangetwo.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                        rangetwo.EntireColumn.AutoFit(); //自动调整列宽
                        rangetwo.Font.Size = 12; //设置字体大小
                        rangetwo.Font.Name = "黑体"; //设置字体的种类 
                        ExcelSheet.Cells[dataTable.Rows.Count + a, 1] = listSt[i];
                    }
                }

                ExcelApp.DisplayAlerts = false; //保存Excel的时候，不弹出是否保存的窗口直接进行保存 

                //弹出保存对话框,并保存文件
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.DefaultExt = ".xlsx";
                sfd.FileName = fileName;
                sfd.Filter = "Office 2007 File|*.xlsx|Office 2000-2003 File|*.xls|所有文件|WPS(*.et)|*.e|*.*";
                if (sfd.ShowDialog() == true)
                {
                    if (sfd.FileName != "")
                    {
                        ExcelBook.SaveAs(sfd.FileName);  //将其进行保存到指定的路径                                                   
                    }
                }
                //释放可能还没释放的进程
                ExcelBook.Close();
                ExcelApp.Quit();
                //  PubHelper.Instance.KillAllExcel(ExcelApp);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }


        private static System.Data.DataTable DataGridToDataTable(System.Windows.Controls.DataGrid dataGrid)
        {
            if (dataGrid == null || dataGrid.Columns.Count <= 0 || dataGrid.ItemsSource == null)
            {
                return null;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visibility == System.Windows.Visibility.Visible)//只导出可见列  
                {
                    dt.Columns.Add(dataGrid.Columns[i].Header.ToString());//构建表头  
                }
            }
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                int columnsIndex = 0;
                System.Data.DataRow row = dt.NewRow();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    if (dataGrid.Columns[j].Visibility == System.Windows.Visibility.Visible)
                    {
                        if (dataGrid.Items[i] != null && (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock) != null)//填充可见列数据  
                        {
                            row[columnsIndex] = (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock).Text.ToString();
                        }
                        else
                        {
                            row[columnsIndex] = "";
                        }
                        columnsIndex++;
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion 导出DataGrid数据到Excel
    }
}
