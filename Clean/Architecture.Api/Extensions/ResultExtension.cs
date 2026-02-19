using Architecture.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Extensions
{
    public static class ResultExtension
    {
        public static IResult ToResult(this Result result)
        {
            return result.IsSuccess ? Results.Ok() : Results.Json(new ProblemDetails
            {
                Status = (int)result.StatusCode,
                Detail = result.Message
            }, statusCode: (int)result.StatusCode);
        }

        public static IResult ToResult<T>(this Result<T> result)
        {
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Json(new ProblemDetails
            {
                Status = (int)result.StatusCode,
                Detail = result.Message
            }, statusCode: (int)result.StatusCode);
        }
    }
}