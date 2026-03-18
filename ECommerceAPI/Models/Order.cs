using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ECommerceAPI.Data;
using ECommerceAPI.Enums;
using ECommerceAPI.Interfaces;

namespace ECommerceAPI.Models
{
    public class Order:IShippable
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public OrderStatus Status {  get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public void SendMessage()
        {
            using var db = new AppDbContext();
            Order order = db.Orders.First(u => u.Id == Id);
            Console.WriteLine("Processing Order....");
            order.Status = OrderStatus.Processing;
            db.SaveChanges();
            Thread.Sleep(5000);
            Console.WriteLine("Shipping Items...");
            order.Status = OrderStatus.Shipped;
            db.SaveChanges();
            Thread.Sleep(7000);
            Console.WriteLine("Delivered!");
            order.Status = OrderStatus.Delivered;
            db.SaveChanges();
        }
        public void Ship()
        {
            Thread thread1 = new Thread(SendMessage);
            thread1.Start();            
        }
    }
}
