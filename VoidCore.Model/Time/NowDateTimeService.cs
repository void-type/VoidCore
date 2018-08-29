namespace VoidCore.Model.Time
{
    /// <summary>
    /// A service for getting the current DateTime.Now.
    /// </summary>
    public class NowDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Returns the current DateTime.
        /// </summary>
        public System.DateTime Moment => System.DateTime.Now;
    }
}
