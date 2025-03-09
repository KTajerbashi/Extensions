using ClosedXML.Excel;
using Serializers.WebApi.Attributes;
using Serializers.WebApi.Controllers;
using System.Reflection;

namespace Serializers.WebApi.EPPlusGenerator;

public class GeneratorParameter
{
    public Type Type { get; set; }
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public int StartColumn { get; set; }
    public int EndColumn { get; set; }
}

public static class ExcelGeneratorWith_ClosedXML
{
    public static string GenerateAndSaveExcelClosedXML<T>(this SheetParameter sheetParameter, string filePath)
    {
        string fileName = $"{Guid.NewGuid()}.xlsx";
        GeneratorParameter parameter = new GeneratorParameter();
        // Create a new workbook
        using (var workbook = new XLWorkbook())
        {
            // Get the SheetParameter type to read its attributes
            var sheetParameterType = typeof(SheetParameter);
            int counter = 0;

            // Get the ExcelSheet attribute from the SheetParameter class
            var sheetAttribute = sheetParameterType.GetCustomAttribute<ExcelSheetAttribute>();

            // Iterate through each sheet in the SheetParameter
            foreach (var sheetEntry in sheetParameter.Datasource)
            {
                var sheetName = sheetEntry.Key;
                var tableParametersList = sheetEntry.Value;

                // Add a worksheet for each sheet
                var worksheet = workbook.Worksheets.Add(sheetName);
                worksheet.RightToLeft = true; // Set the worksheet to RTL
                var personType = typeof(T);
                var properties = personType.GetProperties();
                int row = 1;
                int startColumn = 1; // Starting column for the first table
                int rowCount = 1;
                // Iterate through each TableParameters in the sheet
                int nextColumn = 2;
                foreach (var dataTableEntry in tableParametersList)
                {
                    var tableKey = dataTableEntry.Key; // Use the dictionary key as the table header
                    var people = dataTableEntry.Value;
                    int lastCoumn = (nextColumn + properties.Length);
                    worksheet.Range(2, nextColumn, people.Count, lastCoumn).SetAutoFilter();
                    // Write the table name as a header (using the dictionary key)
                    worksheet.Cell(row, startColumn).Value = tableKey;
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Font.Bold = true;
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Fill.SetBackgroundColor(XLColor.AmberSaeEce); // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Border.BottomBorder = XLBorderStyleValues.Double; // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Border.RightBorder = XLBorderStyleValues.Double; // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Border.LeftBorder = XLBorderStyleValues.Double; // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Merge().Style.Border.TopBorder = XLBorderStyleValues.Double; // Center-align header
                    row++;
                    nextColumn = nextColumn + properties.Length + 2;
                    // Write the column headers (including a "Row Count" column)
                    worksheet.Cell(row, startColumn).Value = "#"; // Add "Row Count" header
                    worksheet.Cell(row, startColumn).Style.Border.LeftBorder = XLBorderStyleValues.Thick; // Add "Row Count" header

                    for (int col = 0; col < properties.Length; col++)
                    {
                        var property = properties[col];
                        var columnAttribute = property.GetCustomAttribute<ExcelColumnAttribute>();
                        worksheet.Cell(row, startColumn + col + 1).Value = columnAttribute?.DisplayName ?? property.Name;
                    }

                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Style.Font.Bold = true;
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Style.Fill.SetBackgroundColor(XLColor.Amber); // Center-align header
                    worksheet.Range(row, startColumn, row, startColumn + properties.Length).Style.Border.BottomBorder = XLBorderStyleValues.Thick; // Center-align header

                    row++;

                    // Apply auto-filter to the header row
                    //worksheet.Range(row, startColumn, row, startColumn + properties.Length).SetAutoFilter();

                    // Write the data rows (including row count)
                    for (int i = 0; i < people.Count; i++)
                    {
                        // Write the row count
                        worksheet.Cell(row, startColumn).Value = rowCount++;
                        worksheet.Cell(row, startColumn).Style.Fill.SetBackgroundColor(XLColor.AirForceBlue);
                        worksheet.Cell(row, startColumn).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(row, startColumn).Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                        worksheet.Cell(row, startColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // Write the person data
                        for (int col = 0; col < properties.Length; col++)
                        {
                            var property = properties[col];
                            var value = property.GetValue(people[i])?.ToString();
                            worksheet.Cell(row, startColumn + col + 1).Value = value;
                            worksheet.Cell(row, startColumn + col + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Center-align header;
                            worksheet.Cell(row, startColumn + col + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        row++;
                    }

                    // Move the starting column for the next table
                    startColumn += properties.Length + 2; // Add 2 columns spacing between tables (1 for row count, 2 for spacing)

                    // Reset the row for the next table
                    row = 1;
                    counter++;
                }
            }


            // Generate a unique file name
            string fullPath = Path.Combine(filePath, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            // Save the Excel file
            FileInfo file = new FileInfo(fullPath);

            // Save the workbook to the specified file path
            workbook.SaveAs(fullPath);
        }
        return fileName;
    }
}