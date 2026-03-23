using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAPI.Services
{
    public class OrderService:IOrderService
    {
        private AppDbContext db;

        public OrderService(AppDbContext db)
        {
            this.db = db;
        }
        public List<Order> GetCustomerOrders(int id)
        {
            List<Order> orders = db.Orders.Where(u => u.CustomerId == id).ToList();
            return orders;
        }

        public bool CreateOrder(int customerId)
        {
            User? db_customer = db.Users.FirstOrDefault(u => u.Id == customerId);
            if(db_customer == null)
            {
                throw new InvalidOperationException($"Customer with id: {customerId} does not exist");
            }

            List<CartItem> cartitems = db.CartItems.Where(u => u.CustomerId == customerId).ToList();
            double total_price = 0;

            if(cartitems.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty");
            }

            foreach (CartItem item in cartitems)
            {
                int productid = item.ProductId;
                Product product = db.Products.First(u => u.Id == productid);
                if (product.Stock - item.Quantity < 0)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                }
                total_price += (product.Price * item.Quantity);
            }
            Order order = new Order();
            order.TotalPrice = total_price;
            order.CustomerId = customerId;
            order.Status = OrderStatus.Pending;
            db.Orders.Add(order);
            db.SaveChanges();

            int orderid = order.Id;


            foreach (CartItem item in cartitems)
            {
                OrderItem orderitem = new OrderItem();
                orderitem.OrderId = orderid;
                orderitem.ProductId = item.ProductId;
                orderitem.Quantity = item.Quantity;

                int productid = item.ProductId;
                Product product = db.Products.First(u => u.Id == productid);
                product.Stock = product.Stock - item.Quantity;
                orderitem.Price = product.Price;
                db.OrderItems.Add(orderitem);

            }
            db.CartItems.RemoveRange(db.CartItems.Where(u => u.CustomerId == customerId));
            db.SaveChanges();

            order.Ship();
            return true;
        }
    }
}
