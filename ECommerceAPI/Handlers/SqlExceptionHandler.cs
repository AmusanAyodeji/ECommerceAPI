using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ECommerceAPI.Handlers
{
    public class SqlExceptionHandler:IExceptionsHandler
    {
        public bool CanHandle(Exception ex)
        {
            return ex is SqlException;
        }
        public ObjectResult Handle(Exception ex)
        {
            return new ObjectResult(new
            {
                StatusCode = 500,
                Message = "A database error occurred"
            })
            { StatusCode = 500 };
        }
    }
}
