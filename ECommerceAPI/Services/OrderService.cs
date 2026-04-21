using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs.Orders;
using ECommerceAPI.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using ECommerceAPI.DTOs.Cart;
using ECommerceAPI.DTOs.Wallet;

namespace ECommerceAPI.Services
{
    public class OrderService:IOrderService
    {
        private AppDbContext db;
        private ILogger<OrderService> _logger;
        private ICartService _cartservice;
        private ICustomerService _customerservice;
        private IProductService _productservice;
        private IWalletService _walletService;
        private IMapper mapper;

        public OrderService(AppDbContext db, ILogger<OrderService> _logger, ICartService _cartservice, ICustomerService _customerservice, IProductService _productservice, IMapper mapper, IWalletService walletService)
        {
            this.db = db;
            this._logger = _logger;
            this._cartservice = _cartservice;
            this._customerservice = _customerservice;
            this._productservice = _productservice;
            this.mapper = mapper;
            this._walletService = walletService;
        }
        public List<OrderResponseDTO> GetCustomerOrders(int id)
        {
            List<OrderResponseDTO> orders = mapper.Map<List<OrderResponseDTO>>(db.Orders.Where(u => u.CustomerId == id).ToList());
            _logger.LogInformation("Found {orders} Orders", orders.Count);
            return orders;
        }

        public bool CreateOrder(int customerId)
        {
            User db_customer = _customerservice.GetById(customerId);
            List<CartItemResponseDTO> cartitemresponse = _cartservice.GetCart(customerId);
            List<CartItem> cartitems = mapper.Map<List<CartItem>>(cartitemresponse);
            double total_price = _productservice.GetTotalPrice(cartitems);
            WalletResponseDTO wallet = _walletService.DisplayWallet(customerId);
            if(!(wallet.amount >= total_price))
            {
                _logger.LogWarning($"Not Enough Balance in Wallet for Customer Id:{customerId}, Total price:{total_price}, Wallet Balance:{wallet.amount}");
                throw new InvalidOperationException("Not Enough Money in Wallet for Transaction");
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
                _productservice.UpdateStockFromCart(item);
                AddOrderItem(orderid, item);
            }
            db.SaveChanges();
            _cartservice.ClearCart(customerId);
            SendMessage(order);
            return true;
        }

        private void AddOrderItem(int orderid, CartItem item)
        {
            Product product = _productservice.GetById(item.ProductId);
            OrderItem orderitem = new OrderItem();
            orderitem.OrderId = orderid;
            orderitem.ProductId = item.ProductId;
            orderitem.Quantity = item.Quantity;
            orderitem.Price = product.Price;
            db.OrderItems.Add(orderitem);
        }
        public void SendMessage(Order order)
        {
            order.Status = OrderStatus.Processing;
            db.SaveChanges();
            _logger.LogInformation("Processing Order for OrderId = {id}", order.Id);
            order.Status = OrderStatus.Shipped;
            db.SaveChanges();
            _logger.LogInformation("Shipping Items for OrderId = {id}", order.Id);
            order.Status = OrderStatus.Delivered;
            db.SaveChanges();
            _logger.LogInformation("Delivered Items for OrderId = {id}", order.Id);
        }
    }
}
