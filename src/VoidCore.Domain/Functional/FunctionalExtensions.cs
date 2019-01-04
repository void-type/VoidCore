using System;

namespace VoidCore.Domain.Functional
{
    /// <summary>
    /// Extension methods that add common functional features for supporting pipelines.
    /// These are similar to LINQ, but go beyond collections.
    /// </summary>
    public static class FunctionalExtensions
    {
        /// <summary>
        /// Map the input to an output. Similar to Select in LINQ.
        /// </summary>
        /// <param name="input">The input</param>
        /// <param name="selector">The map function to transform input to output</param>
        /// <typeparam name="TInput">The input type</typeparam>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <returns></returns>
        public static TOutput Map<TInput, TOutput>(this TInput input, Func<TInput, TOutput> selector)
        {
            return selector(input);
        }

        /// <summary>
        /// Perform a side-effect action while passing the input through to the next step in the pipeline.
        /// This is used for managing side-effects and ensuring the last step's output becomes the next step's input.
        /// </summary>
        /// <param name="input">The input to the tee.</param>
        /// <param name="action">The action to perform.</param>
        /// <typeparam name="T">The type of input.</typeparam>
        /// <returns>The input</returns>
        public static T Tee<T>(this T input, Action<T> action)
        {
            action(input);
            return input;
        }
    }
}
