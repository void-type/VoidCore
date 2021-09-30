namespace VoidCore.Model.Time
{
    /// <summary>
    /// Time splitting overlap mitigation strategy.
    /// </summary>
    public enum OverlapMitigation
    {
        /// <summary>
        /// None - Don't mitigate overlap. Leave this if the end of one range should be the start of the next range.
        /// </summary>
        None,

        /// <summary>
        /// Tick - Add a tick to the next range's start. Good for non-overlapping ranges that use date and time.
        /// </summary>
        Tick,

        /// <summary>
        /// Second - Add a second to the next range's start. Good for non-overlapping ranges that use date and time.
        /// </summary>
        Second,

        /// <summary>
        /// Day - Subtract a day from the end of ranges other than the last. Good for non-overlapping ranges that use only use date.
        /// </summary>
        Day
    }
}
