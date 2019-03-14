using Microsoft.AspNetCore.Mvc;
using VoidCore.Domain;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Files;

namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// Create ActionResults from Results.
    /// </summary>
    public static class HttpResponder
    {
        /// <summary>
        /// Create a ObjectResult based on pass or fail of the domain result. Returns the success value on success.
        /// </summary>
        /// <param name="result">The domain result</param>
        /// <typeparam name="TSuccessValue">The type of success value in the result</typeparam>
        /// <returns>An IActionResult</returns>
        public static IActionResult Respond<TSuccessValue>(IResult<TSuccessValue> result)
        {
            return result.IsSuccess ? Ok(result.Value) : Fail(result);
        }

        /// <summary>
        /// Create an ObjectResult based on pass or fail of the domain result. Returns an empty 200 on success.
        /// </summary>
        /// <param name="result">The domain result</param>
        /// <returns>An IActionResult</returns>
        public static IActionResult Respond(IResult result)
        {
            return result.IsSuccess ? Ok(null) : Fail(result);
        }

        /// <summary>
        /// Respond with an object in an Object result. Always responds with a successful 200.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <returns>An IActionResult</returns>
        public static IActionResult Respond(object obj)
        {
            return Ok(obj);
        }

        /// <summary>
        /// Create a downloadable FileContentResult.
        /// </summary>
        /// <param name="result">The domain result</param>
        /// <returns>An IActionResult</returns>
        public static IActionResult RespondWithFile(IResult<SimpleFile> result)
        {
            if (result.IsFailed)
            {
                return Fail(result);
            }

            var file = result.Value;
            return new FileContentResult(file.Content.AsBytes, "application/force-download") { FileDownloadName = file.Name };

        }

        private static IActionResult Fail(IResult result)
        {
            return new ObjectResult(result.Failures.ToItemSet()) { StatusCode = 400 };
        }

        private static IActionResult Ok(object result)
        {
            return new ObjectResult(result) { StatusCode = 200 };
        }
    }
}
