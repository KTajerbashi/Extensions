using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Extensions.Serializers.Microsoft;

public class SerializerMicrosoft : ISerializerJson, IDisposable
{
    private readonly ILogger<SerializerMicrosoft> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

    public SerializerMicrosoft(ILogger<SerializerMicrosoft> logger)
    {
        _logger = logger;
        _logger.LogInformation("Microsoft Serializer Start working");
    }

    public TOutput Deserialize<TOutput>(string input)
    {
        _logger.LogTrace("Microsoft Serializer Deserialize with name {input}", input);

        return string.IsNullOrWhiteSpace(input) ?
            default : JsonSerializer.Deserialize<TOutput>(input, options);
    }

    public object Deserialize(string input, Type type)
    {
        _logger.LogTrace("Microsoft Serializer Deserialize with name {input} and type {type}", input, type);

        return string.IsNullOrWhiteSpace(input) ?
            default : JsonSerializer.Deserialize(input, type, options);
    }

    public string Serialize<TInput>(TInput input)
    {
        _logger.LogTrace("Microsoft Serializer Serilize with name {input}", input);

        return input == null ? string.Empty : JsonSerializer.Serialize(input, options);
    }

    public void Dispose()
    {
        _logger.LogInformation("Microsoft Serializer Stop working");
    }
}