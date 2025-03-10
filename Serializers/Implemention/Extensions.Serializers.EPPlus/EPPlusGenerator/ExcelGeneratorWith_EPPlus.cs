using DocumentFormat.OpenXml.EMMA;
using Extensions.Serializers.EPPlus.Attributes;
using Extensions.Serializers.EPPlus.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Reflection;

namespace Extensions.Serializers.EPPlus.EPPlusGenerator;

public static class ExcelGeneratorWith_EPPlus
{
    public static string GenerateAndSaveExcelEPPlus<TModel>(this SheetParameter<TModel> sheetParameter, string filePath)
        where TModel : class, new()
    {
        string fileName = $"{Guid.NewGuid()}.xlsx";
        var parameter = GeneratorParameter<TModel>.CreateInstance();
        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        using (var package = new ExcelPackage())
        {
            // Get the SheetParameter type to read its attributes
            parameter.Sheet = typeof(SheetParameter<TModel>);
            parameter.Counter = 0;

            // Get the ExcelSheet attribute from the SheetParameter class
            parameter.SheetAttribute = parameter.Sheet.GetCustomAttribute<ExcelSheetAttribute>();

            // Iterate through each sheet in the SheetParameter
            foreach (var sheetEntry in sheetParameter.Datasource)
            {
                parameter.SheetName = sheetEntry.Key;
                parameter.SheetData = sheetEntry.Value;

                // Add a worksheet for each sheet
                var worksheet = package.Workbook.Worksheets.Add(parameter.SheetName);
                worksheet.View.RightToLeft = true;
                parameter.Type = typeof(TModel);
                parameter.TypeInfo = parameter.Type.GetProperties();
                parameter.RowRecord = 1;
                parameter.StartColumn = 1; // Starting column for the first table
                parameter.RowCounter = 1;

                // Iterate through each TableParameters in the sheet
                foreach (var dataTableEntry in parameter.SheetData)
                {
                    parameter.TableKey = dataTableEntry.Key; // Use the dictionary key as the table header
                    parameter.TableData = dataTableEntry.Value;

                    // Write the table name as a header (using the dictionary key)
                    var headerCell = worksheet.Cells[parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length];
                    headerCell.Value = parameter.TableKey;
                    headerCell.Style.Font.Bold = true;
                    headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCell.Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent1);
                    headerCell.Style.Border.BorderAround(ExcelBorderStyle.Double);

                    // Merge header cells
                    worksheet.Cells[parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length].Merge = true;

                    parameter.RowRecord++;

                    // Write the column headers (including a "Row Count" column)
                    worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Value = "#"; // Add "Row Count" header
                    worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                    for (int col = 0; col < parameter.TypeInfo.Length; col++)
                    {
                        var property = parameter.TypeInfo[col];
                        var columnAttribute = property.GetCustomAttribute<ExcelColumnAttribute>();
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn + col + 1].Value = columnAttribute?.DisplayName ?? property.Name;
                    }

                    // Style the header row
                    var headerRange = worksheet.Cells[parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent2);
                    headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

                    parameter.RowRecord++;

                    // Write the data rows (including row count)
                    for (int i = 0; i < parameter.TableData.Count; i++)
                    {
                        // Write the row count
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Value = parameter.RowCounter++;
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent2);
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells[parameter.RowRecord, parameter.StartColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Write the person data
                        for (int col = 0; col < parameter.TypeInfo.Length; col++)
                        {
                            var property = parameter.TypeInfo[col];
                            var value = property.GetValue(parameter.TableData[i])?.ToString();
                            worksheet.Cells[parameter.RowRecord, parameter.StartColumn + col + 1].Value = value;
                            worksheet.Cells[parameter.RowRecord, parameter.StartColumn + col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[parameter.RowRecord, parameter.StartColumn + col + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Cells[parameter.RowRecord, parameter.StartColumn + col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //worksheet.Cells[parameter.RowRecord, parameter.StartColumn + col + 1].Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Background1);
                        }
                        parameter.RowRecord++;
                    }

                    // Move the starting column for the next table
                    parameter.StartColumn += parameter.TypeInfo.Length + 1; // Add 1 column spacing between tables

                    // Reset the row for the next table
                    parameter.RowRecord = 1;
                    parameter.RowCounter++;
                }

                // Apply auto-filter to the table
                int lastCellRow = parameter.TableData.Count + 2; // +2 for header and sub-header
                int lastCellColumn = (parameter.TypeInfo.Length + 1) * parameter.SheetData.Count;
                worksheet.Cells[2, 1, lastCellRow, lastCellColumn].AutoFilter = true;
            }

            // Generate a unique file name
            string fullPath = Path.Combine(filePath, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // Save the workbook to the specified file path
            package.SaveAs(new FileInfo(fullPath));
        }
        return fileName;
    }
}
