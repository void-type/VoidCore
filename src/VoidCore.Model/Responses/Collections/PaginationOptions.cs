namespace VoidCore.Model.Responses.Collections;

/// <summary>
/// Represents common options to control pagination.
/// </summary>
public class PaginationOptions
{
    private PaginationOptions() { }

    /// <summary>
    /// Create a new pagination options. Out-of-range values are corrected: the page and the take are at least 1, so invalid
    /// request input can't produce a negative offset. There is no upper limit on the take, so validate it if it comes from untrusted input.
    /// </summary>
    /// <param name="page">What page number to take from the set</param>
    /// <param name="take">How many items to include in each page</param>
    /// <param name="isPagingEnabled">If paging is enabled in this set. Default of pagination enabled</param>
    public PaginationOptions(int page, int take, bool isPagingEnabled = true)
    {
        Page = Math.Max(1, page);
        Take = Math.Max(1, take);
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
