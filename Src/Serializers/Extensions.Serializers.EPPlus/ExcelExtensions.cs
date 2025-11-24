using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Data;

namespace Extensions.Serializers.EPPlus;

public static class ExcelExtensions
{
    static ExcelExtensions()
    {
        ExcelPackage.License.SetNonCommercialPersonal("Tajerbashi");
    }

    // ------------------------------------------------------------
    // Convert List<T> → Excel byte[]
    // ------------------------------------------------------------
    public static byte[] ToExcelByteArray<T>(this List<T> list, string sheetName = "Result")
    {
        if (list == null || list.Count == 0)
            return Array.Empty<byte>();

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        worksheet.Cells["A1"].LoadFromCollection(list, PrintHeaders: true, TableStyle: TableStyles.Medium6);
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }

    // ------------------------------------------------------------
    // Excel byte[] → DataTable
    // ------------------------------------------------------------
    public static DataTable ToDataTableFromExcel(this byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return new DataTable();

        using var package = new ExcelPackage(new MemoryStream(bytes));
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet?.Dimension == null)
            return new DataTable();

        var table = new DataTable();
        int rowCount = worksheet.Dimension.Rows;
        int colCount = worksheet.Dimension.Columns;

        // Read header
        for (int col = 1; col <= colCount; col++)
        {
            var header = worksheet.Cells[1, col].Text;
            if (string.IsNullOrWhiteSpace(header))
                header = "Column" + col;

            table.Columns.Add(header);
        }

        // Read data
        for (int row = 2; row <= rowCount; row++)
        {
            var dr = table.NewRow();
            for (int col = 1; col <= colCount; col++)
            {
                dr[col - 1] = worksheet.Cells[row, col].Text;
            }
            table.Rows.Add(dr);
        }

        return table;
    }
}
