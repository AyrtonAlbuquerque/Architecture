using FluentValidation;

namespace Architecture.Api.Extensions
{
    public static class ValidationExtension
    {
        public static RouteHandlerBuilder WithValidation<TRequest>(this RouteHandlerBuilder builder)
        {
            return builder.AddEndpointFilter(async (context, next) =>
            {
                var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
                var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();

                if (validator != null && request != null)
                {
                    var validation = validator.Validate(request);

                    if (!validation.IsValid)
                    {
                        return Results.Problem(new HttpValidationProblemDetails(validation.ToDictionary())
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Validation Error",
                            Detail = "One or more validation errors occurred.",
                            Instance = context.HttpContext.Request.Path
                        });
                    }
                }

                return await next(context);
            });
        }
    }

}