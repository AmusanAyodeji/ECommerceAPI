using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ECommerceAPI.Data;
using ECommerceAPI.Enums;
using ECommerceAPI.Interfaces;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public OrderStatus Status {  get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
