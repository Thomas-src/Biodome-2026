namespace BioDomes.Web.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseLogOrigin(this IApplicationBuilder app)
        =>  app.UseMiddleware<LogOriginMiddleware>();
}