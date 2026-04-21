using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs.Cart;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceAPI.Services
{
    public class CartService:ICartService
    {
        private AppDbContext db;
        private ILogger<CartService> _logger;
        private IValidationHelper _validate;
        private IProductService _productService;
        private IMapper mapper;

        public CartService(AppDbContext db, ILogger<CartService> _logger, IValidationHelper _validate, IProductService _productService, IMapper mapper)
        {
            this.db = db;
            this._logger = _logger;
            this._validate = _validate;
            this._productService = _productService;
            this.mapper = mapper;
        }
        public bool AddToCart(CreateCartItemDTO dTO)
        {
            _validate.CheckLessEqualZero(dTO.quantity, "Cannot Add Product With 0 Quantity Or Less to Cart. ProductId = {productId}");
            Product product = _productService.GetById(dTO.productId);
            if(product == null)
            {
                throw new ArgumentException($"Product with id {dTO.productId} does not exist");
            }
            if(dTO.quantity > product.Stock)
            {
                _logger.LogWarning("Amount is more than available stock");
                throw new InvalidOperationException("Amount is more than available stock");
            }
            CartItem cartitem = mapper.Map<CartItem>(dTO);
            db.CartItems.Add(cartitem);
            db.SaveChanges();
            _logger.LogInformation("Added Product with ProductId = {ProductId}", dTO.productId);
            return true;
        }

        public List<CartItemResponseDTO> GetCart(int customerId)
        {
            List<CartItem> cartitems = db.CartItems.Where(u => u.CustomerId == customerId).ToList();
            List<CartItemResponseDTO> cartitemsresponse = mapper.Map<List<CartItemResponseDTO>>(cartitems);
            _validate.CheckEqualZero(cartitems.Count, $"Cannot Get Cart for customer with id = {customerId}, Cart is empty");
            _logger.LogInformation("Found {Count} Products in Cart", cartitems.Count);
            return cartitemsresponse;
        }

        public bool RemoveFromCart(int cartItemId)
        {
            CartItem? cartitem = db.CartItems.FirstOrDefault(u => u.Id == cartItemId);
            _validate.CheckIfNull(cartItemId, $"Cannot Remove ProductId = {cartItemId} From Cart, Product is not in Cart");
            db.CartItems.Remove(cartitem);
            db.SaveChanges();
            _logger.LogInformation("Removed ProductId = {ProductId} from Cart",cartItemId);
            return true;
        }

        public void ClearCart(int customerId)
        {
            db.CartItems.RemoveRange(db.CartItems.Where(u => u.CustomerId == customerId));
            db.SaveChanges();
            _logger.LogInformation("Cart has been successfully cleared");
        }
    }
}
