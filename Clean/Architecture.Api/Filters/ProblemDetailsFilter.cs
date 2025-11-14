using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Architecture.Api.Filters
{
    public class ProblemDetailsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            void Add(int statusCode)
            {
                if (!operation.Responses.ContainsKey(statusCode.ToString()))
                {
                    operation.Responses[statusCode.ToString()] = new OpenApiResponse
                    {
                        Description = ReasonPhrases.GetReasonPhrase(statusCode),
                        Content = new Dictionary<string, OpenApiMediaType>()
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                            }
                        }
                    };
                }
            }

            Add(StatusCodes.Status400BadRequest);
            Add(StatusCodes.Status500InternalServerError);
        }
    }
}