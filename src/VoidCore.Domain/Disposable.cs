using System;
using System.Threading.Tasks;

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
        /// <typeparam name="TOut">The output type</typeparam>
        /// <returns>The output</returns>
        public static TOut Using<TDisposable, TOut>(Func<TDisposable> factory, Func<TDisposable, TOut> selector)
        where TDisposable : IDisposable
        {
            using(var disposable = factory())
            {
                return selector(disposable);
            }
        }

        /// <summary>
        /// Controls the life-cycle of an IDisposable by wrapping the using block and returning the desired output.
        /// </summary>
        /// <param name="factory">A factory that creates the IDisposable object.</param>
        /// <param name="selector">A map function for using the disposable to get the output.</param>
        /// <typeparam name="TDisposable">The disposable service type</typeparam>
        /// <typeparam name="TOut">The output type</typeparam>
        /// <returns>The output</returns>
        public static async Task<TOut> UsingAsync<TDisposable, TOut>(Func<TDisposable> factory, Func<TDisposable, Task<TOut>> selector)
        where TDisposable : IDisposable
        {
            using(var disposable = factory())
            {
                return await selector(disposable).ConfigureAwait(false);
            }
        }
    }
}
