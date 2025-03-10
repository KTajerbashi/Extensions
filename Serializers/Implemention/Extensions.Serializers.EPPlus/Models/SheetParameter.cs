using DocumentFormat.OpenXml.EMMA;
using Extensions.Serializers.EPPlus.Attributes;

namespace Extensions.Serializers.EPPlus.Models;
public class SheetParameter<TModel> where TModel : class, new()
{
    public SheetParameter()
    {
        Datasource = new();
    }
    [ExcelSheet]
    public Dictionary<string, Dictionary<string, List<TModel>>> Datasource { get; set; }

}
