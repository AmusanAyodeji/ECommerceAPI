using Serilog.Context;
using System.Security.Claims;

namespace ECommerceAPI.Middleware
{
    public class LoggingEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var username = context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
            var role = context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "None";

            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("Username", username))
            using (LogContext.PushProperty("Role", role))
            {
                await _next(context);
            }
        }
    }
}