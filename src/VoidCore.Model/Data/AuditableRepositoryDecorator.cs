using VoidCore.Model.Auth;

namespace VoidCore.Model.Data;

/// <summary>
/// A decorator of a generic repository that updates the audit information on a persisted entity upon creation or modification.
/// </summary>
public class AuditableRepositoryDecorator<T> : RepositoryDecoratorAbstract<T> where T : class, IAuditable
{
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Create a new repo decorator.
    /// </summary>
    /// <param name="innerRepository">The repository to be decorated</param>
    /// <param name="timeProvider">A time provider that supplies the time the entity was updated</param>
    /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
    internal AuditableRepositoryDecorator(IWritableRepository<T> innerRepository, TimeProvider timeProvider, ICurrentUserAccessor currentUserAccessor) : base(innerRepository)
    {
        _timeProvider = timeProvider;
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

    private async Task SetCreated(IAuditable entity)
    {
        entity.SetAuditCreated(_timeProvider.GetUtcNow(), (await _currentUserAccessor.GetUser()).Login);
    }

    private async Task SetModified(IAuditable entity)
    {
        entity.SetAuditModified(_timeProvider.GetUtcNow(), (await _currentUserAccessor.GetUser()).Login);
    }
}
