using Microsoft.AspNetCore.Mvc;
using VoidCore.Model.Action.Railway;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Responses.ItemSet;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Create ActionResults from Results.
    /// </summary>
    public class HttpResponder
    {
        /// <summary>
        /// Create an ObjectResult.
        /// </summary>
        /// <param name="result">The result to send</param>
        /// <typeparam name="TSuccessValue">The type of success value in the result</typeparam>
        /// <returns></returns>
        public IActionResult Respond<TSuccessValue>(Result<TSuccessValue> result)
        {
            if (result.IsSuccess)
            {
                return new ObjectResult(result.Value) { StatusCode = 200 };
            }
            else
            {
                return Failure(result);
            }
        }

        /// <summary>
        /// Create an ObjectResult.
        /// </summary>
        /// <param name="result">The result to send</param>
        /// <returns></returns>
        public IActionResult Respond(Result result)
        {
            if (result.IsSuccess)
            {
                return new ObjectResult(null) { StatusCode = 200 };
            }
            else
            {
                return Failure(result);
            }
        }

        /// <summary>
        /// Create a downloadable FileContentResult.
        /// </summary>
        /// <param name="result">The result to send</param>
        /// <returns></returns>
        public IActionResult RespondWithFile(Result<SimpleFile> result)
        {
            if (result.IsSuccess)
            {
                var file = result.Value;
                return new FileContentResult(file.Content, "application/force-download") { FileDownloadName = file.Name };
            }
            else
            {
                return Failure(result);
            }
        }

        private IActionResult Failure(Result result)
        {
            return new ObjectResult(result.Failures.ToItemSet()) { StatusCode = 400 };
        }
    }
}
