using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAPI.Models
{
    public class Product
    {
        double price;
        int stock;
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get { return price; }
            set
            { 
                if(value > 0)
                {
                    price = value;
                }
                else
                {
                    price = 0;
                }
            }
        }
        public int Stock { get { return stock; }
            set
            {
                if (value > 0) 
                { 
                    stock = value;
                }
                else
                {
                    stock = 0;
                }
            }
        }
        public string? Category { get; set; }
        public int? CategoryId { get; set; }

        public Category? CategoryNav { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public override string ToString()
        {
            string message = "";
            if (CategoryId == null)
            {
                message = $"Name: {Name}, Price: ${Price}, Stock: {Stock}, Category: {Category}";
            }
            else
            {
                message = $"Name: {Name}, Price: ${Price}, Stock: {Stock}, CategoryId: {CategoryId}";
            }
            return message;
        }
    }
}
