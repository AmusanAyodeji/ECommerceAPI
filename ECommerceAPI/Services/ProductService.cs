using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.DTOs.Products;
using ECommerceAPI.Helper;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

namespace ECommerceAPI.Services
{
    public class ProductService:IProductService
    {
        private AppDbContext db;
        private ILogger<ProductService> _logger;
        private IValidationHelper _validate;
        private IMapper mapper;
        public ProductService(AppDbContext db, ILogger<ProductService> _logger, IValidationHelper _validate, IMapper mapper)
        {
            this.db = db;
            this._logger = _logger;
            this._validate = _validate;
            this.mapper = mapper;
        }
        public List<ProductResponseDTO> GetAllProducts()
        {
            _logger.LogInformation("Returned All Products");
            return mapper.Map<List<ProductResponseDTO>>(db.Products.ToList());            
        }

        public ProductResponseDTO? AddProduct(CreateProductDTO productDTO)
        {
            _validate.CheckIfEmpty(productDTO.name, "Product name cannot be empty");
            _validate.CheckIfEmpty(productDTO.category, "Product category cannot be empty");

            Product? db_product = db.Products.FirstOrDefault(u => u.Name == productDTO.name);

            _validate.CheckIfNotNull(db_product, $"Product with name = {productDTO.name} already exists");
            _validate.CheckLessThanZero(productDTO.price, "Product price must be greater than 0");
            _validate.CheckLessThanZero(productDTO.stock, "Product stock cannot be less than 0");

            Product product = mapper.Map<Product>(productDTO);

            db.Products.Add(product);
            db.SaveChanges();
            _logger.LogWarning("Product successfully added to database");
            ProductResponseDTO productresponse = mapper.Map<ProductResponseDTO>(product);
            return productresponse;
        }

        public ProductResponseDTO? AddProductV2(CreateProductV2DTO productDTO)
        {
            _validate.CheckIfEmpty(productDTO.name, "Product name cannot be empty");
            _validate.CheckEqualZero(productDTO.categoryId, "Product category cannot be empty");

            Product? db_product = db.Products.FirstOrDefault(u => u.Name == productDTO.name);
            Category? category = db.Categories.FirstOrDefault(c => c.Id == productDTO.categoryId);

            _validate.CheckIfNull(category, $"Category with id {productDTO.categoryId} not found");
            _validate.CheckIfNotNull(db_product, $"Product with name = {productDTO.name} already exists");
            _validate.CheckLessThanZero(productDTO.price, "Product price must be greater than 0");
            _validate.CheckLessThanZero(productDTO.stock, "Product stock cannot be less than 0");

            Product product = mapper.Map<Product>(productDTO);

            db.Products.Add(product);
            db.SaveChanges();
            _logger.LogInformation("Product successfully added to database");
            ProductResponseDTO productresponse = mapper.Map<ProductResponseDTO>(product);
            return productresponse;
        }

        public bool RemoveProduct(int id)
        {
            Product? db_product = db.Products.FirstOrDefault(u => u.Id == id);
            _validate.CheckIfNull(db_product, $"Product with id {id} does not exist");

            db.Products.Remove(db_product);
            db.SaveChanges();
            _logger.LogInformation("Product removed successfully");
            return true;
        }

        public bool UpdateStock(UpdateStockDTO updateStock)
        {
            _validate.CheckLessThanZero(updateStock.stock, "Product stock cannot be less than 0");
            Product? db_product = db.Products.FirstOrDefault(u => u.Id == updateStock.id);
            _validate.CheckIfNull(db_product, $"Product with id {updateStock.id} does not exist");
            db_product.Stock = updateStock.stock;
            db.SaveChanges();
            _logger.LogInformation("Product with id = {id} stock updated successfully", updateStock.id);
            return true;
        }
        public Product GetById(int Id)
        {
            Product? db_product = db.Products.FirstOrDefault(u => u.Id == Id);
            _validate.CheckIfNull(db_product, $"Product with id: {Id} does not exist");
            return db_product;
        }

        private void CheckProductStock(Product product, CartItem item)
        {
            if (product.Stock - item.Quantity < 0)
            {
                _logger.LogWarning("Not enough stock to process Order");
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
            }
        }

        public void UpdateStockFromCart(CartItem item)
        {
            Product product = GetById(item.ProductId);
            CheckProductStock(product, item);
            UpdateStockDTO updateStock = new UpdateStockDTO();
            updateStock.id = product.Id;
            updateStock.stock = product.Stock - item.Quantity;
            UpdateStock(updateStock);
        }

        public double GetTotalPrice(List<CartItem> cartitems)
        {
            double total_price = 0;
            foreach (CartItem item in cartitems)
            {
                Product product = GetById(item.ProductId);
                CheckProductStock(product, item);
                total_price += (product.Price * item.Quantity);
            }
            return total_price;
        }
    }
}
