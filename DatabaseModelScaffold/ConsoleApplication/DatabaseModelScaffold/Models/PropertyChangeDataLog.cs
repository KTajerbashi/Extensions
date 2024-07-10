using System;
using System.Collections.Generic;

namespace DatabaseModelScaffold.Models;

public partial class PropertyChangeDataLog
{
    public Guid Id { get; set; }

    public Guid? ChangeInterceptorItemId { get; set; }

    public string PropertyName { get; set; } = null!;

    public string? Value { get; set; }

    public virtual EntityChangeDataLog? ChangeInterceptorItem { get; set; }
}
