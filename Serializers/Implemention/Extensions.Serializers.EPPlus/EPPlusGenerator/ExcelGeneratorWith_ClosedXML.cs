using ClosedXML.Excel;
using Extensions.Serializers.EPPlus.Attributes;
using Extensions.Serializers.EPPlus.Models;
using System.Reflection;

namespace Extensions.Serializers.EPPlus.EPPlusGenerator;

public static class ExcelGeneratorWith_ClosedXML
{
    public static string GenerateAndSaveExcelClosedXML<TModel>(this SheetParameter<TModel> sheetParameter, string filePath)
        where TModel : class, new()
    {
        string fileName = $"{Guid.NewGuid()}.xlsx";
        var parameter = GeneratorParameter<TModel>.CreateInstance();

        using (var workbook = new XLWorkbook())
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
                var worksheet = workbook.Worksheets.Add(parameter.SheetName);
                worksheet.RightToLeft = true; // Set the worksheet to RTL
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
                    worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Value = parameter.TableKey;
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Font.Bold = true;
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Fill.SetBackgroundColor(XLColor.AmberSaeEce); // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Border.BottomBorder = XLBorderStyleValues.Double; // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Border.RightBorder = XLBorderStyleValues.Double; // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Border.LeftBorder = XLBorderStyleValues.Double; // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Merge().Style.Border.TopBorder = XLBorderStyleValues.Double; // Center-align header
                    parameter.RowRecord++;

                    // Write the column headers (including a "Row Count" column)
                    worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Value = "#"; // Add "Row Count" header
                    worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Style.Border.LeftBorder = XLBorderStyleValues.Thick; // Add "Row Count" header

                    for (int col = 0; col < parameter.TypeInfo.Length; col++)
                    {
                        var property = parameter.TypeInfo[col];
                        var columnAttribute = property.GetCustomAttribute<ExcelColumnAttribute>();
                        worksheet.Cell(parameter.RowRecord, parameter.StartColumn + col + 1).Value = columnAttribute?.DisplayName ?? property.Name;
                    }

                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Style.Font.Bold = true;
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Style.Fill.SetBackgroundColor(XLColor.Amber); // Center-align header
                    worksheet.Range(parameter.RowRecord, parameter.StartColumn, parameter.RowRecord, parameter.StartColumn + parameter.TypeInfo.Length).Style.Border.BottomBorder = XLBorderStyleValues.Thick; // Center-align header

                    parameter.RowRecord++;

                    // Write the data rows (including row count)
                    for (int i = 0; i < parameter.TableData.Count; i++)
                    {
                        // Write the row count
                        worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Value = parameter.RowCounter++;
                        worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Style.Fill.SetBackgroundColor(XLColor.AirForceBlue);
                        worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                        worksheet.Cell(parameter.RowRecord, parameter.StartColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // Write the person data
                        for (int col = 0; col < parameter.TypeInfo.Length; col++)
                        {
                            var property = parameter.TypeInfo[col];
                            var value = property.GetValue(parameter.TableData[i])?.ToString();
                            worksheet.Cell(parameter.RowRecord, parameter.StartColumn + col + 1).Value = value;
                            worksheet.Cell(parameter.RowRecord, parameter.StartColumn + col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header;
                            worksheet.Cell(parameter.RowRecord, parameter.StartColumn + col + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        parameter.RowRecord++;
                    }

                    // Move the starting column for the next table
                    parameter.StartColumn += parameter.TypeInfo.Length + 1; // Add 1 columns spacing between tables (1 for row count, 2 for spacing)

                    // Reset the row for the next table
                    parameter.RowRecord = 1;
                    parameter.RowCounter++;
                }
                int lastCellRow = parameter.TableData.Count;
                int lastCellColumn = (parameter.TypeInfo.Length + 1) * parameter.SheetData.Count;
                worksheet.Range(2, 1, lastCellRow, lastCellColumn).SetAutoFilter();

            }

            // Generate a unique file name
            string fullPath = Path.Combine(filePath, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // Save the workbook to the specified file path
            workbook.SaveAs(fullPath);
        }
        return fileName;
    }

}