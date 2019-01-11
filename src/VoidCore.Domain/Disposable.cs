using System;

namespace VoidCore.Domain
{
    /// <summary>
    /// Static helpers to make disposable using blocks more functional.
    /// </summary>
    public static class Disposable
    {
        /// <summary>
        /// Controls the life-cycle of an IDisposable by wrapping the using block and returning the desired output.
        /// </summary>
        /// <param name="factory">A factory that creates the IDisposable object.</param>
        /// <param name="selector">A map function for using the disposable to get the output.</param>
        /// <typeparam name="TDisposable">The disposable service type</typeparam>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <returns>The output</returns>
        public static TOutput Using<TDisposable, TOutput>(Func<TDisposable> factory, Func<TDisposable, TOutput> selector)
        where TDisposable : IDisposable
        {
            using(var disposable = factory())
            {
                return selector(disposable);
            }
        }
    }
}
