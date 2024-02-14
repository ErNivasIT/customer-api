using Microsoft.AspNetCore.Mvc.Filters;

namespace Customer_API.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            logger.LogError(context.Exception.Message);
            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}
