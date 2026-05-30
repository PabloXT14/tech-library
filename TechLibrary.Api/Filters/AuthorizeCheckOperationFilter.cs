using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TechLibrary.Api.Filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica se o endpoint ou controller possui o atributo [Authorize]
        var hasAuthorize = context.MethodInfo
                               .GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>()
                               .Any()
                           ||
                           context.MethodInfo.DeclaringType!
                               .GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>()
                               .Any();
        // Verifica se possui [AllowAnonymous] - nesse caso, ignora o [Authorize] herdado
        var hasAllowAnonymous = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .Any();

        if (!hasAuthorize || hasAllowAnonymous)
            return;
        
        // Adiciona o ícone de cadeado apenas nos endpoints com [Authorize]
        operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                // OpenApiSecuritySchemeReference recebe o Id e o documento do contexto
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            }
        ];
    }
}