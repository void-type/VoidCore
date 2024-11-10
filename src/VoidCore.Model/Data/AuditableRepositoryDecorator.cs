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
public class AuditableRepositoryDecorator<T> : RepositoryDecoratorAbstract<T> where T : class, IAuditable
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IDateTimeService _now;

    /// <summary>
    /// Create a new repo decorator.
    /// </summary>
    /// <param name="innerRepository">The repository to be decorated</param>
    /// <param name="now">A datetime service that provides the time the entity was updated</param>
    /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
    internal AuditableRepositoryDecorator(IWritableRepository<T> innerRepository, IDateTimeService now, ICurrentUserAccessor currentUserAccessor) : base(innerRepository)
    {
        _now = now;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <inheritdoc/>
    public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        SetCreated(entity);
        await  InnerRepository.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            SetCreated(entity);
        }

        await  InnerRepository.AddRangeAsync(entitiesList, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        SetModified(entity);
        await  InnerRepository.UpdateAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            SetModified(entity);
        }

        await  InnerRepository.UpdateRangeAsync(entitiesList, cancellationToken);
    }

    private void SetCreated(IAuditable entity)
    {
        entity.SetAuditCreated(_now.Moment, _currentUserAccessor.User.Login);
    }

    private void SetModified(IAuditable entity)
    {
        entity.SetAuditModified(_now.Moment, _currentUserAccessor.User.Login);
    }
}
