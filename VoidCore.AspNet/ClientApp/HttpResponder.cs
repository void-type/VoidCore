using Microsoft.AspNetCore.Mvc;
using VoidCore.Model.ClientApp;
using VoidCore.Model.Railway;
using VoidCore.Model.Railway.File;
using VoidCore.Model.Railway.ItemSet;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Create ActionResults from Results.
    /// </summary>
    public class HttpResponder : IResultResponder<IActionResult>
    {
        /// <summary>
        /// Create a ObjectResult based on pass or fail of the result. Returns the success value on success.
        /// </summary>
        /// <param name="result">The result to send</param>
        /// <typeparam name="TSuccessValue">The type of success value in the result</typeparam>
        /// <returns></returns>
        public IActionResult Respond<TSuccessValue>(Result<TSuccessValue> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return Failure(result);
            }
        }

        /// <summary>
        /// Create an ObjectResult based on pass or fail of the result.
        /// </summary>
        /// <param name="result">The result to send</param>
        /// <returns></returns>
        public IActionResult Respond(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok(null);
            }
            else
            {
                return Failure(result);
            }
        }

        /// <summary>
        /// Respond with an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IActionResult Respond(object obj)
        {
            return Ok(obj);
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

        private IActionResult Ok(object result)
        {
            return new ObjectResult(result) { StatusCode = 200 };
        }
    }
}
