using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace VoidCore.Model.Data;

/// <summary>
/// A decorator of a generic repository that updates the audit information on a persisted entity upon creation or modification.
/// </summary>
public class AuditableWithOffsetRepositoryDecorator<T> : RepositoryDecoratorAbstract<T> where T : class, IAuditableWithOffset
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTimeService _now;

    /// <summary>
    /// Create a new repo decorator.
    /// </summary>
    /// <param name="innerRepository">The repository to be decorated</param>
    /// <param name="now">A datetime service that provides the time the entity was updated</param>
    /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
    internal AuditableWithOffsetRepositoryDecorator(IWritableRepository<T> innerRepository, IDateTimeService now, ICurrentUserAccessor currentUserAccessor) : base(innerRepository)
    {
        _now = now;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <inheritdoc/>
    public override async Task<T> Add(T entity, CancellationToken cancellationToken)
    {
        await SetCreated(entity);
        return await InnerRepository.Add(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            await SetCreated(entity);
        }

        await InnerRepository.AddRange(entitiesList, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task Update(T entity, CancellationToken cancellationToken)
    {
        await SetModified(entity);
        await InnerRepository.Update(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            await SetModified(entity);
        }

        await InnerRepository.UpdateRange(entitiesList, cancellationToken);
    }

    private async Task SetCreated(IAuditableWithOffset entity)
    {
        entity.SetAuditCreated(_now.MomentWithOffset, (await _currentUserAccessor.GetUser()).Login);
    }

    private async Task SetModified(IAuditableWithOffset entity)
    {
        entity.SetAuditModified(_now.MomentWithOffset, (await _currentUserAccessor.GetUser()).Login);
    }
}
