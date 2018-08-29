namespace VoidCore.Model.Time
{
    /// <summary>
    /// An interface for injecting DateTime.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// A getter for the moment provided by the service.
        /// </summary>
        /// <value>The moment provided by the service. Can be static or dynamic with the passing of time.</value>
        System.DateTime Moment { get; }
    }
}
