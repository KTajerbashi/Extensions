﻿using Extensions.Translations.Abstractions;
using System.Data;
using System.Reflection;

namespace Extensions.Serializers.EPPlus.Extensions;
public static class LinqExtensions
{
    public static List<T> ToList<T>(this DataTable dataTable)
    {
        List<T> data = new();

        foreach (DataRow row in dataTable.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }

        return data;
    }

    private static T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);

        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName || column.ColumnName == "")
                    pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
                else
                    continue;
            }
        }

        return obj;
    }
}