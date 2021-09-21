# VoidCore.EntityFramework

[![NuGet package](https://img.shields.io/nuget/v/VoidCore.EntityFramework.svg)](https://www.nuget.org/packages/VoidCore.EntityFramework/)
[![MyGet package](https://img.shields.io/myget/voidcoredev/vpre/VoidCore.EntityFramework.svg?label=myget)](https://www.myget.org/feed/voidcoredev/package/nuget/VoidCore.EntityFramework)

## Installation

```powerShell
dotnet add package VoidCore.EntityFramework
```

## Features

Add an Entity Framework data layer to your application.

### Async Database Abstractions

Included are EntityFramework implementations of the specification and repository patterns from VoidCore.Model. These implementations defer the heavy lifting of searching, sorting, paging, and joining to the SQL Server. All calls are asynchronous and immediate.

Queries are automatically tagged with the repository and specification names they were called with. This helps tie SQL logs backs to the application code. See [Query Tags on Microsoft docs](https://docs.microsoft.com/en-us/ef/core/querying/tags) for more information.

See [VoidCore.Model](model.md) for more about it's data persistence features. You can decorate these EF repositories with Model's auditable and soft-delete decorators.

### Audit and Soft-Delete capabilities when directly using DBContext

Sometimes the specification repository isn't a good fit for a project. Some developers would rather create data services and others don't like adding an abstraction on top of DBContext.

In these cases, you can add the following code to your DBContext partial. This code intercepts the SaveChanges method and calls extension methods on the entities to set audit properties. It will also prevent soft-deletes from actually deleting entities.

You can still use the repository abstractions on top of this DBContext, just don't call the AddAuditability or AddSoftDeletability extension methods when setting up your repositories since the DBContext will handle audit properties.

See a full working example in EfIntegration tests (EfAuditableDbContextTests).

<!-- markdownlint-disable MD033 -->
<details>
    <summary>
        Show code
    </summary>
<!-- markdownlint-disable MD033 -->

```csharp
public partial MyDbContext
{
    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetAuditableAndSoftDeleteProperties();
        SetAuditableAndSoftDeletePropertiesWithOffset();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetAuditableAndSoftDeleteProperties();
        SetAuditableAndSoftDeletePropertiesWithOffset();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditableAndSoftDeleteProperties()
    {
        var now = _dateTimeService.Moment;
        var user = _currentUserAccessor.User.Login;

        var auditableEntityEntries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditable &&
                (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted
                ));

        foreach (var entry in auditableEntityEntries)
        {
            if (entry.State == EntityState.Added)
            {
                ((IAuditable)entry.Entity).SetAuditCreated(now, user);
            }

            if (entry.State == EntityState.Modified)
            {
                ((IAuditable)entry.Entity).SetAuditModified(now, user);
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                ((ISoftDeletable)entry.Entity).SetSoftDeleted(now, user);
            }
        }
    }

    private void SetAuditableAndSoftDeletePropertiesWithOffset()
    {
        var now = _dateTimeService.MomentWithOffset;
        var user = _currentUserAccessor.User.Login;

        var auditableEntityEntries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditableWithOffset &&
                (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted
                ));

        foreach (var entry in auditableEntityEntries)
        {
            if (entry.State == EntityState.Added)
            {
                ((IAuditableWithOffset)entry.Entity).SetAuditCreated(now, user);
            }

            if (entry.State == EntityState.Modified)
            {
                ((IAuditableWithOffset)entry.Entity).SetAuditModified(now, user);
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                ((ISoftDeletableWithOffset)entry.Entity).SetSoftDeleted(now, user);
            }
        }
    }
}
```

</details>
