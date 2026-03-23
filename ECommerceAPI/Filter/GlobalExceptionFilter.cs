using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

namespace ECommerceAPI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            Console.WriteLine($"Exception caught: {ex.Message}");

            if (ex is ArgumentNullException || ex is ArgumentException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
            else if (ex is InvalidOperationException)
            {
                context.Result = new ConflictObjectResult(new
                {
                    StatusCode = 409,
                    Message = ex.Message
                });
            }
            else if (ex is UnauthorizedAccessException)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    StatusCode = 401,
                    Message = ex.Message
                })
                    { StatusCode = 500 };
            }
            else if (ex is SqlException)
            {
                context.Result = new ObjectResult(new
                {
                    StatusCode = 500,
                    Message = "A database error occurred"
                })
                { StatusCode = 500 };
            }
            else
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