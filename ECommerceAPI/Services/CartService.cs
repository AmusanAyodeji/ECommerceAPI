using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
            User? user = db.Users.FirstOrDefault(u => u.Id == customerId);
            Product? products = db.Products.FirstOrDefault(u => u.Id == productId);

            if(user == null || products == null)
            {
                return false;
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
            return cartitems;
        }

        public bool RemoveFromCart(int cartItemId)
        {
            CartItem? cartitem = db.CartItems.FirstOrDefault(u => u.Id == cartItemId);
            if(cartitem == null)
            {
                return false;
            }
            db.CartItems.Remove(cartitem);
            db.SaveChanges();
            return true;
        }
    }
}
