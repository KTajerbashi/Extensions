using Bogus;
using Extensions.Serializers.EPPlus.Attributes;
using Extensions.Serializers.EPPlus.Models;

namespace Serializers.WebApi.Models;
public static class Datasource
{
    public static SheetParameter<TModel> GetSheetParameter<TModel>()
        where TModel : class, new()
    {
        SheetParameter<TModel> data = new();
        data.Datasource.Add("نظر سنجی سامانه میزکار", GetTableParameter<TModel>(1));
        data.Datasource.Add("نظر سنجی سامانه م1یزکار", GetTableParameter<TModel>(2));
        data.Datasource.Add("نظر سنجی سامانه م2یزکار", GetTableParameter<TModel>(3));
        data.Datasource.Add("نظر سنجی سامانه م3یزکار", GetTableParameter<TModel>(4));
        //data.Datasource.Add("Sheet 2", GetTableParameter(3));
        return data;
    }

    public static Dictionary<string, List<TModel>> GetTableParameter<TModel>(int a)
        where TModel : class, new()
    {
        Dictionary<string, List<TModel>> data  = new();
        for (int i = 0; i < a; i++)
        {
            data.Add($"Question{i + 1}", GenerateFakeDataExtensions.GenrateFakeData<TModel>(20));
        }
        return data;
    }


}






public class Person
{
    [ExcelColumn("نام", "Name")]
    public string Name { get; set; }
    [ExcelColumn("فامیل", "Family")]
    public string Family { get; set; }
    [ExcelColumn("کد ملی", "NationalCode")]
    public string NationalCode { get; set; }
    [ExcelColumn("تلفن", "Phone")]
    public string Phone { get; set; }

    [ExcelColumn("تاریخ", "DateTime")]
    public string DateTime { get; set; }

    [ExcelColumn("آدرس", "DateTime")]
    public string Address { get; set; }

    [ExcelColumn("شغل", "DateTime")]
    public string Job { get; set; }

    [ExcelColumn("سمت", "DateTime")]
    public string Role { get; set; }

    [ExcelColumn("تاریخ ایجاد", "CreateDatetTime")]
    public string CreateDatetTime { get; set; }

    [ExcelColumn("تاریخ ویرایش", "UpdateDatetTime")]
    public string UpdateDatetTime { get; set; }
    public Person()
    {

    }

}


