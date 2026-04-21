using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Interfaces;

namespace ECommerceAPI.Handlers
{
    public class ArgumentExceptionHandler: IExceptionsHandler
    {
        public bool CanHandle(Exception ex)
        {
            return ex is ArgumentException || ex is ArgumentNullException;
        }
        public ObjectResult Handle(Exception ex)
        {
            return new BadRequestObjectResult(new
            {
                StatusCode = 400,
                Message = ex.Message
            });
        }
    }
}
