﻿namespace VoidCore.Domain
{
    /// <summary>
    /// Provides convenient non-generic helpers of the generic Maybe class.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Convert an object to a Maybe of obj.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A new Maybe of the type of the object</returns>
        public static Maybe<T> From<T>(T obj)
        {
            return Maybe<T>.From(obj);
        }
    }
}
