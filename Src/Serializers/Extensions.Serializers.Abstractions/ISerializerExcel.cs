using System.Data;

namespace Extensions.Serializers.Abstractions;

public interface ISerializerExcel
{
    byte[] ListToExcelByteArray<T>(List<T> list, string sheetName = "Result");
    DataTable ExcelToDataTable(byte[] bytes);
    List<T> ExcelToList<T>(byte[] bytes) where T : new();
}
