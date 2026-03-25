using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }
        [HttpGet()]
        [Authorize]
        public IActionResult GetCustomerOrders(int id)
        {
            List<Order> orders = _orderService.GetCustomerOrders(id);
            if (orders.Count == 0)
                return NotFound("No orders found for this customer");
            else
                return Ok(orders);
        }

        [HttpPost()]
        [Authorize]
        public IActionResult CreateOrder(int customerid)
        {
            bool order = _orderService.CreateOrder(customerid);
            return Created();
        }
    }
}
