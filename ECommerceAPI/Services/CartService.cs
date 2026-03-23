using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class CartService:ICartService
    {
        private AppDbContext db;

        public CartService(AppDbContext db)
        {
            this.db = db;
        }
        public bool AddToCart(int customerId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity cannot be 0 or less");
            }

            User? user = db.Users.FirstOrDefault(u => u.Id == customerId);
            Product? products = db.Products.FirstOrDefault(u => u.Id == productId);

            if (user == null)
            {
                throw new InvalidOperationException($"User not Found with id: {customerId}");
            }
            if (products == null)
            {
                throw new InvalidOperationException($"Product not Found with id: {productId}");
            }

            CartItem cartitem = new CartItem();
            cartitem.CustomerId = customerId;
            cartitem.ProductId = productId;
            cartitem.Quantity = quantity;
            db.CartItems.Add(cartitem);
            db.SaveChanges();
            return true;
        }

        public List<CartItem> GetCart(int customerId)
        {
            List<CartItem> cartitems = db.CartItems.Where(u => u.CustomerId == customerId).ToList();
            if(cartitems.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty");
            }
            return cartitems;
        }

        public bool RemoveFromCart(int cartItemId)
        {
            CartItem? cartitem = db.CartItems.FirstOrDefault(u => u.Id == cartItemId);
            if (cartitem == null)
            {
                throw new InvalidOperationException($"Cart with id: {cartItemId} not found");
            }
            db.CartItems.Remove(cartitem);
            db.SaveChanges();
            return true;
        }
    }
}
