using FluentResults;
using System.Net;
using System.Text.Json;
using TaskManagement.Application.Common;

namespace TaskManagement.Api.Middleware;

public class ResultExtensions
{
    public static IResult ToApiResponse<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(ApiResponse<T>.SuccessResponse(result.Value, "Operation completed successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return Results.BadRequest(ApiResponse<T>.ErrorResponse("Operation failed", errors));
    }

    public static IResult ToApiResponse(Result result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(ApiResponse.SuccessResponse("Operation completed successfully"));
        }

        var errors = result.Errors.Select(e => e.Message).ToList();
        return Results.BadRequest(ApiResponse.ErrorResponse("Operation failed", errors));
    }
}