using VoidCore.Model.Railway;
using VoidCore.Model.Railway.File;

namespace VoidCore.Model.ClientApp
{
    /// <summary>
    /// Respond out of the domain model with results.
    /// </summary>
    /// <typeparam name="TResponse">They type of response that leaves the model.</typeparam>
    public interface IResultResponder<out TResponse>
    {
        /// <summary>
        /// Respond with typed Result.
        /// </summary>
        /// <param name="result">The result</param>
        /// <typeparam name="TSuccessValue">The type of success value</typeparam>
        /// <returns></returns>
        TResponse Respond<TSuccessValue>(Result<TSuccessValue> result);

        /// <summary>
        /// Respond with a Result.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        TResponse Respond(Result result);

        /// <summary>
        /// Respond with an object.
        /// </summary>
        /// <param name="obj">An object to respond with.</param>
        /// <returns></returns>
        TResponse Respond(object obj);

        /// <summary>
        /// Respond with a Result that may contain a binary file.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        TResponse RespondWithFile(Result<SimpleFile> result);
    }
}
