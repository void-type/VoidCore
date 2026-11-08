using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using VoidCore.Model.Functional;
using VoidCore.Model.Guards;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Files;

namespace VoidCore.AspNet.ClientApp;

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
        result.EnsureNotNull();
        return result.IsSuccess ? Ok(result.Value) : Fail(result);
    }

    /// <summary>
    /// Create an ObjectResult based on pass or fail of the domain result. Returns an empty 200 on success.
    /// </summary>
    /// <param name="result">The domain result</param>
    /// <returns>An IActionResult</returns>
    public static IActionResult Respond(IResult result)
    {
        result.EnsureNotNull();
        return result.IsSuccess ? Ok() : Fail(result);
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
        result.EnsureNotNull();

        if (result.IsFailed)
        {
            return Fail(result);
        }

        return Download(result.Value);
    }

    /// <summary>
    /// Create an inline FileContentResult. This file will not force download. Only content types that can't run script in your site's
    /// origin are shown inline (images other than SVG, audio, video, PDF, and plain text). Anything else, such as HTML or SVG, is sent as
    /// a download so a user-supplied file can't execute as part of your site.
    /// </summary>
    /// <param name="result">The domain result</param>
    /// <returns>An IActionResult</returns>
    public static IActionResult RespondWithInlineFile(IResult<SimpleFile> result)
    {
        result.EnsureNotNull();

        if (result.IsFailed)
        {
            return Fail(result);
        }

        var file = result.Value;

        if (!s_contentTypeProvider.TryGetContentType(file.Name, out var contentType) || !IsSafeToDisplayInline(contentType))
        {
            return Download(file);
        }

        return new FileContentResult(file.Content.AsBytes, contentType);
    }

    private static readonly FileExtensionContentTypeProvider s_contentTypeProvider = new();

    private static bool IsSafeToDisplayInline(string contentType)
    {
        return contentType is "application/pdf" or "text/plain"
            || (contentType.StartsWith("image/", StringComparison.Ordinal) && contentType != "image/svg+xml")
            || contentType.StartsWith("audio/", StringComparison.Ordinal)
            || contentType.StartsWith("video/", StringComparison.Ordinal);
    }

    private static FileContentResult Download(SimpleFile file)
    {
        return new FileContentResult(file.Content.AsBytes, "application/octet-stream")
        {
            FileDownloadName = file.Name
        };
    }

    private static ObjectResult Fail(IResult result)
    {
        return new ObjectResult(result.Failures.ToItemSet()) { StatusCode = 400 };
    }

    // ObjectResult with a null value is turned into a 204 by ASP.NET, so use a real empty 200.
    private static OkResult Ok()
    {
        return new OkResult();
    }

    private static ObjectResult Ok(object result)
    {
        return new ObjectResult(result) { StatusCode = 200 };
    }
}
