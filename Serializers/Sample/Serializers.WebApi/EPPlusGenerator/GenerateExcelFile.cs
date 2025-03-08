using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using Serializers.WebApi.Attributes;
using Serializers.WebApi.Controllers;
using System.Reflection;

namespace Serializers.WebApi.EPPlusGenerator;

public static class GenerateExcelFile
{
    public static string Generate(this MainDatasource dataSource, string path)
    {
        // Set the license context for EPPlus
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var properties = dataSource.GetType().GetProperties();

            foreach (var property in properties)
            {
                var sheetAttribute = property.GetCustomAttribute<ExcelSheetAttribute>();
                if (sheetAttribute != null)
                {
                    var sheet = package.Workbook.Worksheets.Add(sheetAttribute.DisplayName);
                    var datasourceModel = property.GetValue(dataSource) as DatasourceModel;

                    if (datasourceModel != null)
                    {
                        var tables = datasourceModel.GetType().GetProperties();
                        int rowIndex = 1;

                        foreach (var table in tables)
                        {
                            var tableAttribute = table.GetCustomAttribute<ExcelTableAttribute>();
                            if (tableAttribute != null)
                            {
                                sheet.Cells[rowIndex, 1].Value = tableAttribute.DisplayName;
                                rowIndex++;

                                var list = table.GetValue(datasourceModel) as IEnumerable<object>;
                                if (list != null)
                                {
                                    var columns = list.First().GetType().GetProperties();
                                    int colIndex = 1;

                                    foreach (var column in columns)
                                    {
                                        var columnAttribute = column.GetCustomAttribute<ExcelColumnAttribute>();
                                        if (columnAttribute != null)
                                        {
                                            sheet.Cells[rowIndex, colIndex].Value = columnAttribute.DisplayName;
                                            colIndex++;
                                        }
                                    }

                                    rowIndex++;

                                    foreach (var item in list)
                                    {
                                        colIndex = 1;
                                        foreach (var column in columns)
                                        {
                                            var columnAttribute = column.GetCustomAttribute<ExcelColumnAttribute>();
                                            if (columnAttribute != null)
                                            {
                                                sheet.Cells[rowIndex, colIndex].Value = column.GetValue(item)?.ToString();
                                                colIndex++;
                                            }
                                        }
                                        rowIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            // Generate a unique file name
            string fileName = $"{Guid.NewGuid()}.xlsx";
            string fullPath = Path.Combine(path, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Save the Excel file
            FileInfo file = new FileInfo(fullPath);
            package.SaveAs(file);

            return fileName;
        }
    }
}
