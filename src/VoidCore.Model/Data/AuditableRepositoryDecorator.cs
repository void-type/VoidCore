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
        await SetCreated(entity).ConfigureAwait(false);
        return await InnerRepository.Add(entity, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            await SetCreated(entity).ConfigureAwait(false);
        }

        await InnerRepository.AddRange(entitiesList, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task Update(T entity, CancellationToken cancellationToken)
    {
        await SetModified(entity).ConfigureAwait(false);
        await InnerRepository.Update(entity, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task UpdateRange(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            await SetModified(entity).ConfigureAwait(false);
        }

        await InnerRepository.UpdateRange(entitiesList, cancellationToken).ConfigureAwait(false);
    }

    private async Task SetCreated(IAuditable entity)
    {
        var user = await _currentUserAccessor.GetUser().ConfigureAwait(false);
        entity.SetAuditCreated(_timeProvider.GetUtcNow(), user.Login);
    }

    private async Task SetModified(IAuditable entity)
    {
        var user = await _currentUserAccessor.GetUser().ConfigureAwait(false);
        entity.SetAuditModified(_timeProvider.GetUtcNow(), user.Login);
    }
}
