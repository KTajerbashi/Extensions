using System.Reflection;

namespace Extensions.Serializers.EPPlus.EPPlusGenerator;

public class GeneratorParameter<T>
{
    public T Model { get; set; }
    public Type Type { get; set; }
    public PropertyInfo[] TypeInfo { get; set; }
    public Type Sheet { get; set; }
    public Attribute SheetAttribute { get; set; }
    public string SheetName { get; set; }
    public Dictionary<string, List<T>> SheetData { get; set; }
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public int StartColumn { get; set; }
    public int EndColumn { get; set; }
    public int Counter { get; set; }
    public int RowRecord { get; set; }
    public int RowCounter { get; set; }
    public int LastColumn { get; set; }

    public int FirstCellRow { get; set; }
    public int FirstCellColumn { get; set; }
    public int LastCellRow { get; set; }
    public int LastCellColumn { get; set; }

    public string TableKey { get; set; }
    public List<T> TableData { get; set; }

    public static GeneratorParameter<T> CreateInstance()
    {
        return new GeneratorParameter<T>();
    }

}
