using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAPI.Models
{
    
    public class Customer:User
    {
        public List<CartItem> CartItems { get; set; }
        public List<Order> Orders { get; set; }
    }
}
