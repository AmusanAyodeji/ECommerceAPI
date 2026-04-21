using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Handlers
{
    public class InvalidOperationExceptionHandler: IExceptionsHandler
    {
        public bool CanHandle(Exception ex)
        {
            return ex is InvalidOperationException;
        }
        public ObjectResult Handle(Exception ex)
        {
            return new ConflictObjectResult(new
            {
                StatusCode = 409,
                Message = ex.Message
            });
        }
    }
}
