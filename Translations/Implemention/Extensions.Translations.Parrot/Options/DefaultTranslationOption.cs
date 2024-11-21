namespace Extensions.Translations.Parrot.Database;

public class DefaultTranslationOption
{
    public string Key { get; set; } = nameof(Key);
    public string Value { get; set; } = nameof(Value);
    public string Culture { get; set; } = nameof(Culture);
}