using Bogus;

namespace Extensions.Serializers.EPPlus.Models;

public static class GenerateFakeDataExtensions
{
    public static List<TModel> GenrateFakeData<TModel>(int count)
        where TModel : class, new()
    {
        var faker = new Faker<TModel>()
            .CustomInstantiator(f => new TModel()) // Create a new instance of TModel
            .RuleForType(typeof(Guid), f => f.Lorem.Random.Guid()) // Default rule for string properties
            .RuleForType(typeof(string), f => f.Lorem.Word()) // Default rule for string properties
            .RuleForType(typeof(int), f => f.Random.Int()) // Default rule for int properties
            .RuleForType(typeof(bool), f => f.Random.Bool()) // Default rule for int properties
            .RuleForType(typeof(DateTime), f => f.Date.Past()) // Default rule for DateTime properties
            .RuleForType(typeof(DateTimeOffset), f => f.Date.PastOffset()) // Default rule for DateTime properties
              ;
        return faker.Generate(count);
    }
    public static TModel GenrateFakeData<TModel>()
       where TModel : class, new()
    {
        var faker = new Faker<TModel>()
            .CustomInstantiator(f => new TModel()) // Create a new instance of TModel
            .RuleForType(typeof(Guid), f => f.Lorem.Random.Guid()) // Default rule for string properties
            .RuleForType(typeof(string), f => f.Lorem.Word()) // Default rule for string properties
            .RuleForType(typeof(int), f => f.Random.Int()) // Default rule for int properties
            .RuleForType(typeof(bool), f => f.Random.Bool()) // Default rule for int properties
            .RuleForType(typeof(DateTime), f => f.Date.Past()) // Default rule for DateTime properties
            .RuleForType(typeof(DateTimeOffset), f => f.Date.PastOffset()) // Default rule for DateTime properties
              ;
        return faker.Generate(1).First();
    }
}