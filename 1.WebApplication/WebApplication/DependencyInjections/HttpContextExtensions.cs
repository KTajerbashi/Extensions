using Application.Layer.Model.Base.Commands;
using Application.Layer.Model.Base.Events;
using Application.Layer.Model.Base.Queries;

namespace WebApplicationAPI.DependencyInjections;

/// <summary>
/// 
/// </summary>
public static class HttpContextExtensions
{
    public static ICommandDispatcher CommandDispatcher(this HttpContext httpContext) =>
        (ICommandDispatcher)httpContext.RequestServices.GetService(typeof(ICommandDispatcher));

    public static IQueryDispatcher QueryDispatcher(this HttpContext httpContext) =>
        (IQueryDispatcher)httpContext.RequestServices.GetService(typeof(IQueryDispatcher));
    public static IEventDispatcher EventDispatcher(this HttpContext httpContext) =>
        (IEventDispatcher)httpContext.RequestServices.GetService(typeof(IEventDispatcher));
    //public static UtilitiesServices ApplicationContext(this HttpContext httpContext) =>
    //    (UtilitiesServices)httpContext.RequestServices.GetService(typeof(UtilitiesServices));

}