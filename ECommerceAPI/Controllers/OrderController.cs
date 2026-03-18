using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("getcustomerorders")]
        public IActionResult GetCustomerOrders(int id)
        {
            List<Order> orders = _orderService.GetCustomerOrders(id);
            if (orders.Count == 0)
                return NotFound("No orders found for this customer");
            else
                return Ok(orders);
        }

        [HttpPost("createorder")]
        public IActionResult CreateOrder(int customerid)
        {
            bool order = _orderService.CreateOrder(customerid);
            if(order == true)
            {
                return Created();
            }
            else
            {
                return BadRequest("An Error Occurred When Creating Order");
            }
        }
    }
}
