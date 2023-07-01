using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JailTalk.Api.Filters;

public class SwaggerBearerTokenFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Find the "Authorization" parameter in the operation's parameters
        var authParameter = operation.Parameters?.FirstOrDefault(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

        if (authParameter != null)
        {
            // Append the Bearer prefix to the parameter's default value
            authParameter.Description = "Bearer " + authParameter.Description;
        }
    }
}
