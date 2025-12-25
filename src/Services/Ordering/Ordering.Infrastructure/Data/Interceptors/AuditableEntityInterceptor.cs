

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors
{
    //explain use of this class
    // This class is an implementation of a SaveChangesInterceptor in Entity Framework Core.
    // It is used to automatically update auditing properties (like CreatedBy, CreatedAt, LastModifiedBy, LastModified)
    // on entities that implement the IEntity interface whenever changes are being saved to the database.
    // The interceptor overrides the SavingChanges and SavingChangesAsync methods to inject
    // the auditing logic before the changes are committed.

    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        //this method is used to update the auditing properties of entities before saving changes
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            //context is the DbContext instance that is being used to track changes to entities.
            //changeTracker is a property of DbContext that provides access to the entities being tracked by the context.
            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "Gaurav";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModifiedBy = "Gaurav";
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }

    }

    // extension method to check if any owned entities have changed
    // this is useful for determining if the parent entity should be considered modified
    // when owned entities are added or modified
    // Owned entities are types that do not have their own identity and are typically
    // used to represent value objects within an entity.
    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
