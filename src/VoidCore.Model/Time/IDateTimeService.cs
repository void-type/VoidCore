using System;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// An interface for injecting time.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// A getter for the moment provided by the service. Provides a DateTime.
        /// </summary>
        /// <value>The moment provided by the service. Can be static or dynamic with the passing of time.</value>
        DateTime Moment { get; }


        /// <summary>
        /// A getter for the moment provided by the service. Provides a DateTimeOffset.
        /// </summary>
        /// <value>The moment provided by the service. Can be static or dynamic with the passing of time.</value>
        DateTimeOffset MomentWithOffset { get; }
    }
}
