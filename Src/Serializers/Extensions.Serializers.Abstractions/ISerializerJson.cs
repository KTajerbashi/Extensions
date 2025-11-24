namespace Extensions.Serializers.Abstractions;

public interface ISerializerJson
{
    string Serialize<TInput>(TInput input);
    TOutput Deserialize<TOutput>(string input);
    object Deserialize(string input, Type type);
}