using System.Net;
using System.Text.Json;
using FluentResults;
using TaskManagement.Application.Common;

namespace TaskManagement.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            UnauthorizedAccessException => new ErrorResponse("Unauthorized", HttpStatusCode.Unauthorized),
            InvalidOperationException => new ErrorResponse(exception.Message, HttpStatusCode.BadRequest),
            ArgumentException => new ErrorResponse(exception.Message, HttpStatusCode.BadRequest),
            FluentValidation.ValidationException validationEx => new ErrorResponse(
                "Validation failed",
                HttpStatusCode.BadRequest,
                validationEx.Errors.Select(e => e.ErrorMessage).ToList()),
            _ => new ErrorResponse("An internal server error occurred", HttpStatusCode.InternalServerError)
        };

        response.StatusCode = (int)errorResponse.StatusCode;

        var apiResponse = ApiResponse.ErrorResponse(errorResponse.Message, errorResponse.Errors);
        var result = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
}

public class ErrorResponse
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> Errors { get; set; }

    public ErrorResponse(string message, HttpStatusCode statusCode, List<string>? errors = null)
    {
        Message = message;
        StatusCode = statusCode;
        Errors = errors ?? new List<string>();
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}