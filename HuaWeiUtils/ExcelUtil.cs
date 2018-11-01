using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HuaWeiUtils
{
    public class ExcelUtil
    {
        /// <summary>
        /// 将数据集合保存到指定的Excel文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelPath"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public static bool SaveModelToExcel<T>(string excelPath, IEnumerable<T> modelList)
        {
            if (modelList != null && modelList.GetEnumerator().MoveNext())
            {
                Type type = typeof(T);
                // 获取该类型名称，其作为解析XML时筛选节点的XPath
                string className = type.ToString().Split('.').Last();

                // 获取类型的所有字段和属性
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                FileStream excelFile = null;
                HSSFWorkbook workbook = new HSSFWorkbook();
                try
                {
                    ISheet sheet = workbook.CreateSheet(className);

                    ICellStyle headercellStyle = workbook.CreateCellStyle();
                    headercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    headercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    headercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    headercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    headercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    // 字体
                    NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
                    headerfont.Boldweight = (short)FontBoldWeight.Bold;
                    headercellStyle.SetFont(headerfont);

                    int columnIndex = 0;
                    IRow headerRow = sheet.CreateRow(0);
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        ICell cell = headerRow.CreateCell(columnIndex);
                        cell.SetCellValue(fieldInfo.Name);
                        cell.CellStyle = headercellStyle;
                        columnIndex += 1;
                    }
                    foreach (PropertyInfo property in properties)
                    {
                        ICell cell = headerRow.CreateCell(columnIndex);
                        cell.SetCellValue(property.Name);
                        cell.CellStyle = headercellStyle;
                        columnIndex += 1;
                    }

                    ICellStyle cellStyle = workbook.CreateCellStyle();
                    // 为避免日期格式被Excel自动替换，所以设定
                    // format 为 『@』 表示一率当成 text 來看
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                    NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                    cellfont.Boldweight = (short)FontBoldWeight.Normal;
                    cellStyle.SetFont(cellfont);

                    // 建立内容行
                    int rowIndex = 1;
                    int cellIndex = 0;
                    foreach (var model in modelList)
                    {
                        IRow DataRow = sheet.CreateRow(rowIndex);
                        // 反射模型的字段
                        foreach (FieldInfo fieldInfo in fieldInfos)
                        {
                            ICell cell = DataRow.CreateCell(cellIndex);
                            object fieldValue = fieldInfo.GetValue(model);
                            if (fieldValue == null)
                            {
                                cell.SetCellValue("NULL");
                            }
                            else
                            {
                                cell.SetCellValue(fieldValue.ToString());
                            }
                            cell.CellStyle = cellStyle;
                            cellIndex += 1;
                        }
                        // 反射模型的属性
                        foreach (PropertyInfo property in properties)
                        {
                            ICell cell = DataRow.CreateCell(cellIndex);

                            object propValue = property.GetValue(model, null);
                            if (propValue == null)
                            {
                                cell.SetCellValue("NULL");
                            }
                            else
                            {
                                cell.SetCellValue(propValue.ToString());
                            }
                            cell.CellStyle = cellStyle;
                            cellIndex += 1;
                        }
                        cellIndex = 0;
                        rowIndex += 1;
                    }

                    // 自适应列宽度
                    for (int i = 0; i < columnIndex; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                    // 写Excel
                    excelFile = new FileStream(excelPath, FileMode.OpenOrCreate);
                    workbook.Write(excelFile);
                    excelFile.Flush();

                    return true;
                }
                catch { }
                finally
                {
                    if (excelPath != null)
                    {
                        excelFile.Close();
                    }
                }
            }
            return false;
        }
    }
}
