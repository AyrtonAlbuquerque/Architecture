using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Api.Extensions
{
    public static class ResultExtension
    {
        public static IResult Problem(this Result result)
        {
            return result.IsFailed ? Results.Problem(new ProblemDetails
            {
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Errors } }
                }
            }) : throw new InvalidOperationException("It is not possible to convert a successful result into a problem");
        }

        public static IResult Problem<T>(this Result<T> result)
        {
            return result.IsFailed ? Results.Problem(new ProblemDetails
            {
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Errors } }
                }
            }) : throw new InvalidOperationException("It is not possible to convert a successful result into a problem");
        }
    }
}