﻿using Extensions.ChangeDataLog.Abstractions;
using Extensions.ChangeDataLog.Hamster.Options;
using Extensions.UsersManagement.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;


namespace Extensions.ChangeDataLog.Hamster.Interceptors;

public sealed class AddChangeDataLogInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SaveEntityChangeLogs(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SaveEntityChangeLogs(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SaveEntityChangeLogs(DbContextEventData eventData)
    {
        var changeTracker = eventData.Context.ChangeTracker;
        var userInfoService = eventData.Context.GetService<IUserManager<long>>();
        var itemRepository = eventData.Context.GetService<IEntityChageInterceptorItemRepository>();
        var options = eventData.Context.GetService<IOptions<ChangeDataLogHamsterOptions>>().Value;
        var changedEntities = GetChangedEntities(changeTracker);
        var transactionId = Guid.NewGuid().ToString();
        var dateOfAccured = DateTime.Now;

        var entityChangeInterceptorItems = new List<EntityChageInterceptorItem>();

        foreach (var entity in changedEntities)
        {
            var entityChangeInterceptorItem = new EntityChageInterceptorItem
            {
                Id = Guid.NewGuid(),
                TransactionId = transactionId,
                UserId = userInfoService.UserId.ToString(),
                Ip = userInfoService.Ip,
                EntityType = entity.Entity.GetType().FullName,
                EntityId = entity.Property(options.BusinessIdFieldName).CurrentValue.ToString(),
                DateOfOccurrence = dateOfAccured,
                ChangeType = entity.State.ToString(),
                ContextName = GetType().FullName
            };

            foreach (var property in entity.Properties.Where(c => options.PropertyForReject.All(d => d != c.Metadata.Name)))
            {
                if (entity.State == EntityState.Added || property.IsModified)
                {
                    entityChangeInterceptorItem.PropertyChangeLogItems.Add(new PropertyChangeLogItem
                    {
                        ChageInterceptorItemId = entityChangeInterceptorItem.Id,
                        PropertyName = property.Metadata.Name,
                        Value = property.CurrentValue?.ToString(),
                    });
                }
            }

            // Only add the interceptor item if there are property changes
            if (entityChangeInterceptorItem.PropertyChangeLogItems.Count != 0)
            {
                entityChangeInterceptorItems.Add(entityChangeInterceptorItem);
            }
        }

        if (entityChangeInterceptorItems.Count != 0)
        {
            var currentTransaction = changeTracker.Context.Database.CurrentTransaction;

            if (currentTransaction == null)
                throw new InvalidOperationException("No active database transaction found.");

            // Access the underlying DbTransaction
            var dbTransaction = currentTransaction.GetDbTransaction();

            itemRepository.Save(entityChangeInterceptorItems, dbTransaction);
        }
    }

    private List<EntityEntry> GetChangedEntities(ChangeTracker changeTracker) =>
        changeTracker.Entries()
            .Where(x => (x.State == EntityState.Modified
                      || x.State == EntityState.Added
                      || x.State == EntityState.Deleted)
                      && !x.Metadata.IsOwned()) // Exclude owned types
            .ToList();
}
