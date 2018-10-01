using Microsoft.AspNetCore.Mvc;
using VoidCore.Model.ClientApp;
using VoidCore.Model.DomainEvents;
using VoidCore.Model.Responses.File;
using VoidCore.Model.Responses.Collections;

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
            return result.IsSuccess ? Ok(result.Value) : Failure(result);
        }

        /// <summary>
        /// Create an ObjectResult based on pass or fail of the result.
        /// </summary>
        /// <param name="result">The result to send</param>
        /// <returns></returns>
        public IActionResult Respond(Result result)
        {
            return result.IsSuccess ? Ok(null) : Failure(result);
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
        public IActionResult RespondWithFile(Result<ISimpleFile> result)
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

        private static IActionResult Failure(IResult result)
        {
            return new ObjectResult(result.Failures.ToItemSet()) { StatusCode = 400 };
        }

        private static IActionResult Ok(object result)
        {
            return new ObjectResult(result) { StatusCode = 200 };
        }
    }
}
