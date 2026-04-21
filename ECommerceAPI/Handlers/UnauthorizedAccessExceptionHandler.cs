using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Handlers
{
    public class UnauthorizedAccessExceptionHandler: IExceptionsHandler
    {
        public bool CanHandle(Exception ex)
        {
            return ex is UnauthorizedAccessException;
        }
        public ObjectResult Handle(Exception ex)
        {
            return new UnauthorizedObjectResult(new
            {
                StatusCode = 401,
                Message = ex.Message
            })
            { StatusCode = 500 };
        }
    }
}
