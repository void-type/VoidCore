using System;
using System.Threading.Tasks;

namespace VoidCore.Domain
{
    /// <summary>
    /// Extensions for the Result class. Adapted from https://github.com/vkhorikov/CSharpFunctionalExtensions
    /// </summary>
    public static class ResultExtensionsTee
    {
        /// <summary>
        /// If the result is failed, perform a side-effect action then pass the original result through to the next step
        /// in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static IResult TeeOnFailure(this IResult result, Action action)
        {
            if (result.IsFailed)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is failed, asynchronously perform a side-effect action then pass the original result through to
        /// the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult> TeeOnFailureAsync(this IResult result, Func<Task> actionTask)
        {
            if (result.IsFailed)
            {
                await actionTask().ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// If the result is failed, asynchronously perform a side-effect action then pass the original result through to
        /// the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult> TeeOnFailureAsync(this Task<IResult> resultTask, Action action)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.TeeOnFailure(action);
        }

        /// <summary>
        /// If the result is failed, asynchronously perform a side-effect action then pass the original result through to
        /// the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult> TeeOnFailureAsync(this Task<IResult> resultTask, Func<Task> actionTask)
        {
            var result = await resultTask.ConfigureAwait(false);

            return await result.TeeOnFailureAsync(actionTask).ConfigureAwait(false);
        }

        /// <summary>
        /// If the result is failed, perform a side-effect action then pass the original result through to the next step
        /// in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static IResult<T> TeeOnFailure<T>(this IResult<T> result, Action action)
        {
            if (result.IsFailed)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is failed, asynchronously perform a side-effect action then pass the original result through to
        /// the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnFailureAsync<T>(this IResult<T> result, Func<Task> actionTask)
        {
            if (result.IsFailed)
            {
                await actionTask().ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// If the result is failed, asynchronously perform a side-effect action then pass the original result through to
        /// the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnFailureAsync<T>(this Task<IResult<T>> resultTask, Action action)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.TeeOnFailure(action);
        }

        /// <summary>
        /// If the result is failed, asynchronously perform a side-effect action then pass the original result through to
        /// the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnFailureAsync<T>(this Task<IResult<T>> resultTask, Func<Task> actionTask)
        {
            var result = await resultTask.ConfigureAwait(false);

            return await result.TeeOnFailureAsync(actionTask).ConfigureAwait(false);
        }

        /// <summary>
        /// If the result is successful, perform a side-effect action then pass the original result through to the next
        /// step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static IResult TeeOnSuccess(this IResult result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult> TeeOnSuccessAsync(this IResult result, Func<Task> actionTask)
        {
            if (result.IsSuccess)
            {
                await actionTask().ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult> TeeOnSuccessAsync(this Task<IResult> resultTask, Action action)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.TeeOnSuccess(action);
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult> TeeOnSuccessAsync(this Task<IResult> resultTask, Func<Task> actionTask)
        {
            var result = await resultTask.ConfigureAwait(false);

            return await result.TeeOnSuccessAsync(actionTask).ConfigureAwait(false);
        }

        /// <summary>
        /// If the result is successful, perform a side-effect action then pass the original result through to the next
        /// step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static IResult<T> TeeOnSuccess<T>(this IResult<T> result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnSuccessAsync<T>(this IResult<T> result, Func<Task> actionTask)
        {
            if (result.IsSuccess)
            {
                await actionTask().ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnSuccessAsync<T>(this Task<IResult<T>> resultTask, Action action)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.TeeOnSuccess(action);
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnSuccessAsync<T>(this Task<IResult<T>> resultTask, Func<Task> actionTask)
        {
            var result = await resultTask.ConfigureAwait(false);

            return await result.TeeOnSuccessAsync(actionTask).ConfigureAwait(false);
        }

        /// <summary>
        /// If the result is successful, perform a side-effect action then pass the original result through to the next
        /// step in the pipeline. This side-effect takes the result value as a parameter.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to perform</param>
        /// <typeparam name="T">The value of the result</typeparam>
        /// <returns>The original result</returns>
        public static IResult<T> TeeOnSuccess<T>(this IResult<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnSuccessAsync<T>(this IResult<T> result, Func<T, Task> actionTask)
        {
            if (result.IsSuccess)
            {
                await actionTask(result.Value).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="action">The action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnSuccessAsync<T>(this Task<IResult<T>> resultTask, Action<T> action)
        {
            var result = await resultTask.ConfigureAwait(false);

            return result.TeeOnSuccess(action);
        }

        /// <summary>
        /// If the result is successful, asynchronously perform a side-effect action then pass the original result
        /// through to the next step in the pipeline.
        /// </summary>
        /// <param name="resultTask">An asynchronous task representing the the result</param>
        /// <param name="actionTask">The asynchronous action to perform</param>
        /// <returns>The original result</returns>
        public static async Task<IResult<T>> TeeOnSuccessAsync<T>(this Task<IResult<T>> resultTask, Func<T, Task> actionTask)
        {
            var result = await resultTask.ConfigureAwait(false);

            return await result.TeeOnSuccessAsync(actionTask).ConfigureAwait(false);
        }
    }
}
