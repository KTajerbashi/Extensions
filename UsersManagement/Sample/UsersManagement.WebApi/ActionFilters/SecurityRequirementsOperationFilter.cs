using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace UsersManagement.WebApi.ActionFilters;

//// Operation filter to add security requirements
//public class SecurityRequirementsOperationFilter : IOperationFilter
//{
//    public void Apply(OpenApiOperation operation, OperationFilterContext context)
//    {
//        var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
//            .Union(context.MethodInfo.GetCustomAttributes(true))
//            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>();

//        //if (authAttributes.Any())
//        //{
//        //    var securityRequirement = new OpenApiSecurityRequirement();

//        //    // Check for JWT/JWE
//        //    if (context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(em => em is ))
//        //    {
//        //        securityRequirement.Add(
//        //            new OpenApiSecurityScheme
//        //            {
//        //                Reference = new OpenApiReference
//        //                {
//        //                    Type = ReferenceType.SecurityScheme,
//        //                    Id = "Bearer"
//        //                }
//        //            },
//        //            new List<string>()
//        //        );
//        //    }
//        //    // Check for Cookie auth
//        //    else if (context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(em => em is Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware))
//        //    {
//        //        securityRequirement.Add(
//        //            new OpenApiSecurityScheme
//        //            {
//        //                Reference = new OpenApiReference
//        //                {
//        //                    Type = ReferenceType.SecurityScheme,
//        //                    Id = "CookieAuth"
//        //                }
//        //            },
//        //            new List<string>()
//        //        );
//        //    }
//        //    // Check for Session auth
//        //    else if (context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(em => em is Microsoft.AspNetCore.Session.SessionMiddleware))
//        //    {
//        //        securityRequirement.Add(
//        //            new OpenApiSecurityScheme
//        //            {
//        //                Reference = new OpenApiReference
//        //                {
//        //                    Type = ReferenceType.SecurityScheme,
//        //                    Id = "SessionAuth"
//        //                }
//        //            },
//        //            new List<string>()
//        //        );
//        //    }

//        //    if (securityRequirement.Count > 0)
//        //    {
//        //        operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
//        //    }
//        //}
    
//    }
//}






// And add this class somewhere in your project
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>();

        if (authAttributes.Any())
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                }
            };
        }
    }
}

