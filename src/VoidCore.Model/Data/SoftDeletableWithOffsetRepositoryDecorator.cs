using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace VoidCore.Model.Data;

/// <summary>
/// A decorator of a generic repository that soft deletes an entity rather than fully deleting from persistence.
/// </summary>
public class SoftDeletableWithOffsetRepositoryDecorator<T> : RepositoryDecoratorAbstract<T> where T : class, ISoftDeletableWithOffset
{
    private readonly IDateTimeService _now;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    /// <summary>
    /// Create a new repo decorator.
    /// </summary>
    /// <param name="innerRepository">The repository to be decorated</param>
    /// <param name="now">A datetime service that provides the time the entity was updated</param>
    /// <param name="currentUserAccessor">An accessor for the current user's properties</param>
    internal SoftDeletableWithOffsetRepositoryDecorator(IWritableRepository<T> innerRepository, IDateTimeService now, ICurrentUserAccessor currentUserAccessor) : base(innerRepository)
    {
        _now = now;
        _currentUserAccessor = currentUserAccessor;
    }

    /// <inheritdoc/>
    public override async Task Remove(T entity, CancellationToken cancellationToken)
    {
        await SetDeleted(entity);
        await InnerRepository.Update(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            await SetDeleted(entity);
        }

        await InnerRepository.UpdateRange(entitiesList, cancellationToken);
    }

    private async Task SetDeleted(ISoftDeletableWithOffset entity)
    {
        entity.SetSoftDeleted(_now.MomentWithOffset, (await _currentUserAccessor.GetUser()).Login);
    }
}
