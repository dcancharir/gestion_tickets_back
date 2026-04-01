using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Filters;

// Filters/SecurityRequirementsOperationFilter.cs
public class SecurityRequirementsOperationFilter : IOperationFilter {
    public void Apply(OpenApiOperation operation, OperationFilterContext context) {
        var hasAuthorize = context.MethodInfo.DeclaringType!
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any()
            ||
            context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any();

        if(!hasAuthorize)
            return;

        var securityRequirement = new OpenApiSecurityRequirement();

        securityRequirement.Add(
            new OpenApiSecuritySchemeReference("Bearer", null),
            new List<string>()
        );

        operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
    }
}
