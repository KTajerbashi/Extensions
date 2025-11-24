using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Extensions.Serializers.NewtonSoft;

public sealed class SerializerNewtonSoft : ISerializerJson, IDisposable
{
    private readonly ILogger<SerializerNewtonSoft> _logger;

    // Reusable settings instance (much faster than creating every call)
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public SerializerNewtonSoft(ILogger<SerializerNewtonSoft> logger)
    {
        _logger = logger;
        _logger.LogInformation("NewtonSoft Serializer started.");
    }

    public TOutput Deserialize<TOutput>(string input)
    {
        _logger.LogTrace("Deserialize<TOutput> called. Input: {input}", input);

        if (string.IsNullOrWhiteSpace(input))
            return default!;

        return JsonConvert.DeserializeObject<TOutput>(input)!;
    }

    public object Deserialize(string input, Type type)
    {
        _logger.LogTrace("Deserialize called. Input: {input}, Type: {type}", input, type);

        if (string.IsNullOrWhiteSpace(input) || type == null)
            return default!;

        return JsonConvert.DeserializeObject(input, type)!;
    }

    public string Serialize<TInput>(TInput input)
    {
        _logger.LogTrace("Serialize<TInput> called. Input: {@input}", input);

        if (input is null)
            return string.Empty;

        return JsonConvert.SerializeObject(input, JsonSettings);
    }

    public void Dispose()
    {
        _logger.LogInformation("NewtonSoft Serializer stopped.");
    }
}
