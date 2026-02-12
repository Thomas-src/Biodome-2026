namespace BioDomes.Web.Middlewares;

public class LogOriginMiddleware : IMiddleware
{
    private readonly ILogger<LogOriginMiddleware> _logger;

    public LogOriginMiddleware(ILogger<LogOriginMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Incoming request from {ipAddress}", context.Connection.RemoteIpAddress);

        await next(context);
    }
}