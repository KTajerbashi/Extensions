
using Extensions.Serializers.Abstractions;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Extensions.Serializers.EPPlus;

public class SerializerEPPlusExcel : ISerializerExcel
{
    static SerializerEPPlusExcel()
    {
        ExcelPackage.License.SetNonCommercialPersonal("Tajerbashi");
    }
    // ------------------------------------------------------------
    // Excel → DataTable
    // ------------------------------------------------------------
    public DataTable ExcelToDataTable(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            throw new Exception("Excel file is empty.");

        //ExcelPackage.License.SetNonCommercialPersonal("Tajerbashi");
        using var package = new ExcelPackage(new MemoryStream(bytes));
        var worksheet = package.Workbook.Worksheets.FirstOrDefault()
            ?? throw new Exception("No worksheet found in Excel file.");

        var dt = new DataTable();
        var dim = worksheet.Dimension;

        // Columns
        for (int col = 1; col <= dim.End.Column; col++)
        {
            var columnName = worksheet.Cells[1, col].Text;
            if (string.IsNullOrWhiteSpace(columnName))
                columnName = $"Column{col}";

            dt.Columns.Add(columnName);
        }

        // Rows
        for (int row = 2; row <= dim.End.Row; row++)
        {
            var dr = dt.NewRow();
            for (int col = 1; col <= dim.End.Column; col++)
            {
                dr[col - 1] = worksheet.Cells[row, col].Text;
            }
            dt.Rows.Add(dr);
        }

        return dt;
    }

    // ------------------------------------------------------------
    // Excel → List<T>
    // ------------------------------------------------------------
    public List<T> ExcelToList<T>(byte[] bytes) where T : new()
    {
        var dt = ExcelToDataTable(bytes);
        var list = new List<T>();

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (DataRow row in dt.Rows)
        {
            var item = new T();
            foreach (var prop in props)
            {
                if (!dt.Columns.Contains(prop.Name)) continue;

                var rawValue = row[prop.Name];
                if (rawValue == DBNull.Value) continue;

                try
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    var safeValue = Convert.ChangeType(rawValue, targetType);
                    prop.SetValue(item, safeValue);
                }
                catch
                {
                    // Ignore conversion errors
                }
            }
            list.Add(item);
        }

        return list;
    }

    // ------------------------------------------------------------
    // List<T> → Excel byte[]
    // ------------------------------------------------------------
    public byte[] ListToExcelByteArray<T>(List<T> list, string sheetName = "Result")
    {
        if (list == null || list.Count == 0)
            return Array.Empty<byte>();


        //ExcelPackage.License.SetNonCommercialPersonal("Tajerbashi");

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Header
        for (int col = 0; col < props.Length; col++)
        {
            worksheet.Cells[1, col + 1].Value = props[col].Name;
        }

        // Data
        for (int row = 0; row < list.Count; row++)
        {
            for (int col = 0; col < props.Length; col++)
            {
                worksheet.Cells[row + 2, col + 1].Value = props[col].GetValue(list[row]);
            }
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }
}