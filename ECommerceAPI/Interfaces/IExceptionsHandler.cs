using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Interfaces
{
    public interface IExceptionsHandler
    {
        bool CanHandle(Exception ex);
        ObjectResult Handle(Exception ex);
    }
}
