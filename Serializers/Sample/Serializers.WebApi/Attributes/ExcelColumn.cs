﻿namespace Serializers.WebApi.Attributes;

public class ExcelColumnAttribute : Attribute
{
    public string DisplayName { get; set; }
    public string Name { get; set; }
    public ExcelColumnAttribute(string displayName, string name)
    {
        DisplayName = displayName;
        Name = name;
    }
}

public class ExcelSheetAttribute : Attribute
{
}
public class ExcelTableAttribute : Attribute
{
}
