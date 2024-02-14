public class RequestLoggerMiddleware
{
    private readonly ILogger<RequestLoggerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation($"Request path: {context.Request.Path}");
        await _next(context);
        _logger.LogInformation($"Completed Request path: {context.Request.Path}");
    }
}