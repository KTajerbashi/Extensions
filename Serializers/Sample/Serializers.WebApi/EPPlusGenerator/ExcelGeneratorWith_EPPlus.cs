using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serializers.WebApi.Attributes;
using Serializers.WebApi.Controllers;
using System.Reflection;

namespace Serializers.WebApi.EPPlusGenerator;

public static class ExcelGeneratorWith_EPPlus
{
    public static string GenerateAndSaveExcelEPPlus<T>(this SheetParameter sheetParameter, string filePath)
    {
        // Set EPPlus license context (required for non-commercial use)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        string fileName = $"{Guid.NewGuid()}.xlsx";

        // Create a new Excel package
        using (var excelPackage = new ExcelPackage())
        {
            // Get the SheetParameter type to read its attributes
            var sheetParameterType = typeof(SheetParameter);

            // Iterate through each sheet in the SheetParameter
            foreach (var sheetEntry in sheetParameter.Datasource)
            {
                var sheetName = sheetEntry.Key;
                var tableParametersList = sheetEntry.Value;

                // Add a worksheet for each sheet
                var worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                worksheet.View.RightToLeft = true; // Set the worksheet to RTL

                var personType = typeof(T);
                var properties = personType.GetProperties();
                int row = 1;
                int startColumn = 1; // Starting column for the first table
                int rowCount = 1;

                // Iterate through each TableParameters in the sheet
                foreach (var dataTableEntry in tableParametersList)
                {
                    var tableKey = dataTableEntry.Key; // Use the dictionary key as the table header
                    var people = dataTableEntry.Value;

                    // Write the table name as a header (using the dictionary key)
                    worksheet.Cells[row, startColumn].Value = tableKey;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Merge = true;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Font.Bold = true;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Chocolate);
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Border.Right.Style = ExcelBorderStyle.Double;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Border.Left.Style = ExcelBorderStyle.Double;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Border.Top.Style = ExcelBorderStyle.Double;

                    row++;

                    // Write the column headers (including a "Row Count" column)
                    worksheet.Cells[row, startColumn].Value = "#"; // Add "Row Count" header
                    worksheet.Cells[row, startColumn].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                    for (int col = 0; col < properties.Length; col++)
                    {
                        var property = properties[col];
                        var columnAttribute = property.GetCustomAttribute<ExcelColumnAttribute>();
                        worksheet.Cells[row, startColumn + col + 1].Value = columnAttribute?.DisplayName ?? property.Name;
                    }

                    // Style the header row
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Font.Bold = true;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Chocolate);
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

                    // Apply auto-filter to the header row
                    worksheet.Cells[row, startColumn, row, startColumn + properties.Length].AutoFilter = true;

                    row++;

                    // Write the data rows (including row count)
                    for (int i = 0; i < people.Count; i++)
                    {
                        // Write the row count
                        worksheet.Cells[row, startColumn].Value = rowCount++;
                        worksheet.Cells[row, startColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, startColumn].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                        worksheet.Cells[row, startColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[row, startColumn].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        worksheet.Cells[row, startColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Write the person data
                        for (int col = 0; col < properties.Length; col++)
                        {
                            var property = properties[col];
                            var value = property.GetValue(people[i])?.ToString();
                            worksheet.Cells[row, startColumn + col + 1].Value = value;
                            worksheet.Cells[row, startColumn + col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[row, startColumn + col + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        row++;
                    }

                    // Move the starting column for the next table
                    startColumn += properties.Length + 2; // Add 2 columns spacing between tables (1 for row count, 2 for spacing)

                    // Reset the row for the next table
                    row = 1;
                }
            }

            // Generate a unique file name
            string fullPath = Path.Combine(filePath, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // Save the Excel file
            FileInfo file = new FileInfo(fullPath);
            excelPackage.SaveAs(file);
        }

        return fileName;
    }
}
