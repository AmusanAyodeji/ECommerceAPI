using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
