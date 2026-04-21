using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using ECommerceAPI.Interfaces;

namespace ECommerceAPI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private ILogger<GlobalExceptionFilter> _logger;
        private IEnumerable<IExceptionsHandler> handlers;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> _logger, IEnumerable<IExceptionsHandler> handlers)
        {
            this._logger = _logger;
            this.handlers = handlers;
        }
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError($"Exception caught: {ex.Message}");

            IExceptionsHandler? handler = handlers.FirstOrDefault(u => u.CanHandle(ex));
            if(handler != null)
            {
                context.Result = handler.Handle(ex);
            }else
            {
                context.Result = new ObjectResult(new
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred"
                })
                { StatusCode = 500 };
            }
            context.ExceptionHandled = true;
        }
    }
}