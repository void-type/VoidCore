namespace VoidCore.Model.Responses.Collections;

/// <summary>
/// Represents common options to control pagination.
/// </summary>
public class PaginationOptions
{
    /// <summary>
    /// The largest page size allowed. Larger requested sizes are reduced to this. Disable pagination to get an entire set.
    /// </summary>
    public const int MaxTake = 1000;

    private PaginationOptions() { }

    /// <summary>
    /// Create a new pagination options. Out-of-range values are corrected: the page is at least 1 and the take is
    /// between 1 and <see cref="MaxTake"/>, so unvalidated request input can't produce invalid or unbounded queries.
    /// </summary>
    /// <param name="page">What page number to take from the set</param>
    /// <param name="take">How many items to include in each page</param>
    /// <param name="isPagingEnabled">If paging is enabled in this set. Default of pagination enabled</param>
    public PaginationOptions(int page, int take, bool isPagingEnabled = true)
    {
        Page = Math.Max(1, page);
        Take = Math.Clamp(take, 1, MaxTake);
        IsPagingEnabled = isPagingEnabled;
    }

    /// <summary>
    /// Get a default pagination options that disables pagination.
    /// </summary>
    public static readonly PaginationOptions None = new();

    /// <summary>
    /// What page number to take from the set.
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// How many items to include in each page.
    /// </summary>
    public int Take { get; }

    /// <summary>
    /// Should pagination be performed. If false, the whole set is included.
    /// </summary>
    public bool IsPagingEnabled { get; }

    /// <summary>
    /// How many items to skip to reach the start of the page. Doesn't overflow for very large page numbers.
    /// </summary>
    public int Skip => (int)Math.Min((long)(Math.Max(1, Page) - 1) * Take, int.MaxValue);
}
