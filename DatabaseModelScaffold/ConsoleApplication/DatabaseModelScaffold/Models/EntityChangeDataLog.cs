using System;
using System.Collections.Generic;

namespace DatabaseModelScaffold.Models;

public partial class EntityChangeDataLog
{
    public Guid Id { get; set; }

    public string ContextName { get; set; } = null!;

    public string EntityType { get; set; } = null!;

    public string EntityId { get; set; } = null!;

    public string? UserId { get; set; }

    public string? Ip { get; set; }

    public string? TransactionId { get; set; }

    public DateTime DateOfOccurrence { get; set; }

    public string? ChangeType { get; set; }

    public virtual ICollection<PropertyChangeDataLog> PropertyChangeDataLogs { get; set; } = new List<PropertyChangeDataLog>();
}
