using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using Pal.Core.Domains.Companies;
using Pal.Core.Domains.Customers;
using Pal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Path = System.IO.Path;

namespace Pal.Services.ExcelManager
{
    public class Excel : IExcel
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        public Excel(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        public DataTable DocumentDataTable(string path)
        {
            var constr = _configuration.GetConnectionString("excelconnection");
            DataTable datatable = new DataTable();

            constr = string.Format(constr, path);

            using (OleDbConnection excelconn = new OleDbConnection(constr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter adapterexcel = new OleDbDataAdapter())
                    {

                        excelconn.Open();
                        cmd.Connection = excelconn;
                        DataTable excelschema;
                        excelschema = excelconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        var sheetname = excelschema.Rows[0]["Table_Name"].ToString();
                        excelconn.Close();

                        excelconn.Open();
                        cmd.CommandText = "SELECT * From [" + sheetname + "]";
                        adapterexcel.SelectCommand = cmd;
                        adapterexcel.Fill(datatable);
                        excelconn.Close();

                    }
                }

            }

            return datatable;
        }

        public string Documentupload(IFormFile fromFiles)
        {
            string uploadpath = _environment.WebRootPath;
            string dest_path = System.IO.Path.Combine(uploadpath, "uploaded_doc");

            if (!Directory.Exists(dest_path))
            {
                Directory.CreateDirectory(dest_path);
            }
            string sourcefile = Path.GetFileName(fromFiles.FileName);
            string path = Path.Combine(dest_path, sourcefile);

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                fromFiles.CopyTo(filestream);
            }
            return path;
        }


        //// Given a document name and text, 
        //// inserts a new worksheet and writes the text to cell "A1" of the new worksheet.
        //public static void InsertDataToExcel(string docName, string text)
        //{
        //    // Open the document for editing.
        //    using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
        //    {
        //        // Get the SharedStringTablePart. If it does not exist, create a new one.
        //        SharedStringTablePart shareStringPart;
        //        if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
        //        {
        //            shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
        //        }
        //        else
        //        {
        //            shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
        //        }

        //        // Insert the text into the SharedStringTablePart.
        //        int index = InsertSharedStringItem(text, shareStringPart);

        //        // Insert a new worksheet.
        //        WorksheetPart worksheetPart = InsertWorksheet(spreadSheet.WorkbookPart);

        //        // Insert cell A1 into the new worksheet.
        //        Cell cell = InsertCellInWorksheet("A", 1, worksheetPart);

        //        // Set the value of cell A1.
        //        cell.CellValue = new CellValue(index.ToString());
        //        cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

        //        // Save the new worksheet.
        //        worksheetPart.Worksheet.Save();
        //    }
        //}

        //static void WriteExcelFile(string cmdText)
        //{

        //    DataTable table = new DataTable();
        //    using (SqlConnection connection = new SqlConnection(ConnectionStrings.AppConnectionString))
        //    {
        //        SqlDataAdapter adapter = new SqlDataAdapter
        //        {
        //            SelectCommand = new SqlCommand(cmdText, connection)
        //        };
        //        adapter.Fill(table);
        //    }

        //    // List<Company> persons = new List<Company>();
        //    // { new UserDetails() {ID="1001", Name="ABCD", City ="City1", Country="USA"} };
        //    // DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(persons), (typeof(DataTable)));

        //    using (SpreadsheetDocument document = SpreadsheetDocument.Create("TestNewData.xlsx", SpreadsheetDocumentType.Workbook))
        //    {
        //        WorkbookPart workbookPart = document.AddWorkbookPart();
        //        workbookPart.Workbook = new Workbook();

        //        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        //        var sheetData = new SheetData();
        //        worksheetPart.Worksheet = new Worksheet(sheetData);

        //        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
        //        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 2, Name = "Resources" };

        //        sheets.Append(sheet);

        //        Row headerRow = new Row();

        //        List<String> columns = new List<string>();
        //        foreach (System.Data.DataColumn column in table.Columns)
        //        {
        //            columns.Add(column.ColumnName);

        //            Cell cell = new Cell();
        //            cell.DataType = CellValues.String;
        //            cell.CellValue = new CellValue(column.ColumnName);
        //            headerRow.AppendChild(cell);
        //        }

        //        sheetData.AppendChild(headerRow);

        //        foreach (DataRow dsrow in table.Rows)
        //        {
        //            Row newRow = new Row();
        //            foreach (String col in columns)
        //            {
        //                Cell cell = new Cell();
        //                cell.DataType = CellValues.String;
        //                cell.CellValue = new CellValue(dsrow[col].ToString());
        //                newRow.AppendChild(cell);
        //            }

        //            sheetData.AppendChild(newRow);
        //        }

        //        workbookPart.Workbook.Save();
        //    }

        //}



        public DataTable ImportDocument(DataTable dt, string cmdText, DataTable subDt)
        {
            //subDt = subDt != null ? subDt : new DataTable();

            using (var connection = new SqlConnection(ConnectionStrings.AppConnectionString))
            {
                var command = new SqlCommand(cmdText, connection);
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        foreach (DataColumn col in dt.Columns) {
                            if (row[col.ColumnName] == null) break;
                            command.Parameters.AddWithValue($"@{col.ColumnName}", row[col.ColumnName]);
                        }
                        connection.Open();

                        var id = (int)command.ExecuteScalar();
                        int indx = dt.Rows.IndexOf(row);
                        if (subDt != null) { subDt.Rows[indx].SetField("Id", id); }

                        connection.Close();
                        command.Parameters.Clear(); 
                    }
                    catch(Exception ex) 
                    {
                        break;
                    }
                }
                
            }
            return subDt;
        }

        // https://stackoverflow.com/questions/70084112/save-excel-file-to-csv-format-without-opening-in-net-core-3-1
        public DataTable GetDataFromExcel(string path, dynamic worksheet)
        {
            //Save the uploaded Excel file.

            DataTable dt = new DataTable();

            // Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(path))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(worksheet);

                //Create a new DataTable.

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            if (!string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            else
                            {
                                break;
                            }
                        }
                        firstRow = false;
                    }
                    else
                    {
                        int i = 0;
                        DataRow toInsert = dt.NewRow();
                        foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                        {
                            try
                            {
                                toInsert[i] = cell.Value.ToString();
                            }
                            catch (Exception ex)
                            {
                                //Handle this, or don't.
                            }
                            i++;
                        }
                        dt.Rows.Add(toInsert);
                    }
                }
                return dt;
            }

        }

        // https://www.c-sharpcorner.com/article/c-sharp-convert-excel-to-datatable/
        public DataTable excelToDt(string filePath)
        {
            DataTable dt = new DataTable();

            var fi = new FileInfo(filePath);
            // Check if the file exists
            if (!fi.Exists)
                throw new Exception("File " + filePath + " Does Not Exists");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var xlPackage = new ExcelPackage(fi);
            // get the first worksheet in the workbook
            var worksheet = xlPackage.Workbook.Worksheets[0];
            
            dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
            {
                c.DataTableName = worksheet.Name;
                c.FirstRowIsColumnNames = true;
            });

            //using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filePath, false))
            //{
            //    WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
            //    IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            //    dt.TableName = sheets.First().Name;
            //    string relationshipId = sheets.First().Id.Value;
            //    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
            //    Worksheet workSheet = worksheetPart.Worksheet;
            //    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
            //    IEnumerable<Row> rows = sheetData.Descendants<Row>();

            //    foreach (Cell cell in rows.ElementAt(0))
            //    {
            //        if (cell.CellValue != null)
            //        {
            //            var col = dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
            //            if (col.ColumnName == "CompanyCategoryId") { col.DataType = typeof(int); }
            //        }
            //    }

            //    try
            //    {
            //        foreach (Row row in rows.Skip(1)) 
            //        {
            //            DataRow tempRow = dt.NewRow();
            //            for (int i = 0; i < row.Descendants<Cell>().Count() -1; i++)
            //            {
            //                string val = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
            //                object rslt = val;
            //                if (dt.Columns[i].DataType == typeof(int)) { rslt = int.Parse(val); };
            //                tempRow[i] = rslt;
            //            }
            //            dt.Rows.Add(tempRow);
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        throw;
            //    }
            //}
            return dt;
        }

        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {

            try
            {
                SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
                string value = cell.CellValue == null ? "" : cell.CellValue.InnerXml;

                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    //var x = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    return value;
                }
            }
            catch (Exception)
            {

                return "";
            }

        }




    }
}

