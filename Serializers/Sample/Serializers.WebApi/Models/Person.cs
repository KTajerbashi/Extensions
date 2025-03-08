using Bogus;
using Serializers.WebApi.Attributes;

namespace Serializers.WebApi.Controllers;


public static class Datasource
{
    public static MainDatasource GetMainDatasource()
    {
        return new MainDatasource
        {
            Sheet1 = GetDatasourceModel(),
            Sheet2 = GetDatasourceModel()
        };
    }
    public static DatasourceModel GetDatasourceModel()
    {
        return new DatasourceModel
        {
            People = GetPeople(),
            Students = GetStudent()
        };
    }

    public static List<Person> GetPeople()
    {
        var fakePerson = new Faker<Person>()
            .RuleFor(p => p.Name, f => f.Name.FirstName())
            .RuleFor(p => p.Family, f => f.Name.LastName())
            .RuleFor(p => p.NationalCode, f => f.Random.Replace("##########"))
            .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber("09##########"))
            .RuleFor(p => p.DateTime, f => f.Date.Past().ToString("yyyy-MM-dd HH:mm:ss"));

        return fakePerson.Generate(20);
    }

    public static List<Student> GetStudent()
    {
        var fakeStudent = new Faker<Student>()
            .RuleFor(s => s.Name, f => f.Name.FirstName())
            .RuleFor(s => s.Family, f => f.Name.LastName())
            .RuleFor(s => s.NationalCode, f => f.Random.Replace("##########"))
            .RuleFor(s => s.Phone, f => f.Phone.PhoneNumber("09##########"))
            .RuleFor(s => s.DateTime, f => f.Date.Past().ToString("yyyy-MM-dd HH:mm:ss"));

        return fakeStudent.Generate(20);
    }
}


public class MainDatasource
{
    [ExcelSheet("File A", "اطلاعات سازمان فاوا")]
    public DatasourceModel Sheet1 { get; set; }
    [ExcelSheet("File B", "اطلاعات شهرداری")]
    public DatasourceModel Sheet2 { get; set; }

}

public class DatasourceModel
{
    [ExcelTable("افراد", "People")]
    public List<Person> People { get; set; }

    [ExcelTable("دانشجویان", "Students")]
    public List<Student> Students { get; set; }
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
}


public class Student
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
}
