using ECommerceAPI.Enums;
using ECommerceAPI.Models;
using System.Collections;

namespace ECommerceAPI.DTOs.Orders
{
    public class OrderResponseDTO
    {
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; }
        public override string ToString()
        {
            string message = $"Customer ID: {CustomerId}, Price: ${TotalPrice}, Created At: {CreatedAt}, Order Status: {Status}";
            return message;
        }
    }
}
