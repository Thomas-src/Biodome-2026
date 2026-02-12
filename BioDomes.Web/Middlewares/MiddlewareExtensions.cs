namespace BioDomes.Web.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseLogOrigin(this IApplicationBuilder app)
        =>  app.UseMiddleware<LogOriginMiddleware>();

    public static IApplicationBuilder UseReverseProxy(this IApplicationBuilder app)
        => app.UseMiddleware<ReverseProxyLinksMiddleware>();
}