﻿using Extensions.Translations.Parrot.Database;

namespace Extensions.Translations.Parrot.Options;

public class ParrotTranslatorOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public bool AutoCreateSqlTable { get; set; } = true;
    public string TableName { get; set; } = "ParrotTranslations";
    public string SchemaName { get; set; } = "dbo";
    public int ReloadDataIntervalInMinuts { get; set; }
    public DefaultTranslationOption[] DefaultTranslations { get; set; } = Array.Empty<DefaultTranslationOption>();

}
