using Bogus;
using Serializers.WebApi.Attributes;

namespace Serializers.WebApi.Controllers;
public static class Datasource
{
    public static SheetParameter GetSheetParameter()
    {
        SheetParameter data = new();
        data.Datasource.Add("نظر سنجی سامانه میزکار", GetTableParameter(5));
        //data.Datasource.Add("Sheet 2", GetTableParameter(3));
        return data;
    }

    public static Dictionary<string, List<Person>> GetTableParameter(int a)
    {
        Dictionary<string, List<Person>> data  = new();
        for (int i = 0; i < a; i++)
        {
            data.Add($"Question{i}", GetPeople());
        }
        return data;
    }

    public static List<Person> GetPeople()
    {
        var fakePerson = new Faker<Person>()
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Family, f => f.Name.LastName())
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.Role, f => f.Rant.Locale)
            .RuleFor(p => p.Job, f => f.Company.Bs())
            .RuleFor(p => p.NationalCode, f => f.Random.Replace("##########"))
            .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber("09##########"))
            .RuleFor(p => p.DateTime, f => f.Date.Past().ToString("yyyy-MM-dd HH:mm:ss"));

        return fakePerson.Generate(20);
    }
}



public class SheetParameter
{
    public SheetParameter()
    {
        Datasource = new();
    }
    [ExcelSheet]
    public Dictionary<string, Dictionary<string, List<Person>>> Datasource { get; set; }

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

}


