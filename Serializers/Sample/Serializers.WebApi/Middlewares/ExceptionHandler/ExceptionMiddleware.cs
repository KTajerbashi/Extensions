using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serializers.WebApi.Exceptions;

namespace Serializers.WebApi.Middlewares.ExceptionHandler
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            try
            {
                return _next(httpContext);

            }
            catch (Exception ex)
            {
                int statusCode = ex switch
                {
                    InvalidMainDatasourceException or
                    InvalidDatasourceException or
                    InvalidTableException or
                    InvalidColumnException 
                    => 3351,
                    _ => 200
                };
                httpContext.Response.StatusCode = statusCode;
                throw;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
