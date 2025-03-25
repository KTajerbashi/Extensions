using Extensions.Serializers.Abstractions;
using Extensions.Serializers.EPPlus.Extensions;
using Extensions.Translations.Abstractions;
using System.Data;

namespace Extensions.Serializers.EPPlus.Services;

public class EPPlusExcelSerializer : IExcelSerializer
{
    public EPPlusExcelSerializer()
    {
    }

    public byte[] ListToExcelByteArray<T>(List<T> list, string sheetName = "Result") => list.ToExcelByteArray(sheetName);

    public DataTable ExcelToDataTable(byte[] bytes) => bytes.ToDataTableFromExcel();

    public List<T> ExcelToList<T>(byte[] bytes) => bytes.ToDataTableFromExcel().ToList<T>();
}
