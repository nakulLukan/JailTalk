using JailTalk.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace JailTalk.Infrastructure.Impl.Middlewares;

public class AppApiGlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<AppApiGlobalExceptionHandlerMiddleware> logger;

    public AppApiGlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppApiGlobalExceptionHandlerMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppApiException ex)
        {
            logger.LogError(ex, "Exception encountered");

            var problemDetails = new ProblemDetails
            {
                Title = ex.ErrorCode,
                Status = (int)ex.StatusCode,
                Detail = ex.Message,
            };
            await WriteErrorResponse(context, ex.StatusCode, problemDetails);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");

            var problemDetails = new ProblemDetails
            {
                Title = "ISE",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "Oops, something went wrong."
            };
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, problemDetails);
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, ProblemDetails problemDetails)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}