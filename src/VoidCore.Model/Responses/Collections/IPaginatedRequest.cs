namespace VoidCore.Model.Responses.Collections;

/// <summary>
/// A request that contains pagination options.
/// </summary>
public interface IPaginatedRequest
{
    /// <summary>
    /// What page number to take from the set.
    /// </summary>
    int Page { get; }

    /// <summary>
    /// How many items to include in each page.
    /// </summary>
    int Take { get; }

    /// <summary>
    /// Should pagination be performed. If false, the whole set is included.
    /// </summary>
    bool IsPagingEnabled { get; }
}
