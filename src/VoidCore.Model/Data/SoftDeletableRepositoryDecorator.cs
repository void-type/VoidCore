﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace VoidCore.Model.Data;

/// <summary>
/// A decorator of a generic repository that soft deletes an entity rather than fully deleting from persistence.
/// </summary>
public class SoftDeletableRepositoryDecorator<T> : RepositoryDecoratorAbstract<T> where T : class, ISoftDeletable
{
    private readonly IDateTimeService _now;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    /// <summary>
    /// Create a new repo decorator.
    /// </summary>
    /// <param name="innerRepository">The repository to be decorated</param>
    /// <param name="now">A datetime service that provides the time the entity was updated</param>
    /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
    internal SoftDeletableRepositoryDecorator(IWritableRepository<T> innerRepository, IDateTimeService now, ICurrentUserAccessor currentUserAccessor) : base(innerRepository)
    {
        _now = now;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <inheritdoc/>
    public override Task Remove(T entity, CancellationToken cancellationToken)
    {
        SetDeleted(entity);
        return InnerRepository.Update(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            SetDeleted(entity);
        }

        return InnerRepository.UpdateRange(entitiesList, cancellationToken);
    }

    private void SetDeleted(ISoftDeletable entity)
    {
        entity.SetSoftDeleted(_now.Moment, _currentUserAccessor.User.Login);
    }
}
