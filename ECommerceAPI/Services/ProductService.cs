using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

namespace ECommerceAPI.Services
{
    public class ProductService:IProductService
    {
        private AppDbContext db;
        public ProductService(AppDbContext db)
        {
            this.db = db;
        }
        public List<Product> GetAllProducts()
        {
            return db.Products.ToList();            
        }

        public Product? AddProduct(string name, double price, int stock, string category)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Product name cannot be empty");
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category", "Product category cannot be empty");

            if (price <= 0)
            {
                throw new ArgumentException("Product price must be greater than 0");
            }
            if (stock < 0)
            {
                throw new ArgumentException("Product stock cannot be less than 0");
            }
            Product product = new Product();
            product.Name = name;
            product.Price = price;
            product.Stock = stock;
            product.Category = category;
            Product? db_product = db.Products.FirstOrDefault(u => u.Name == name);
            if (db_product != null)
            {
                throw new InvalidOperationException("Product already exists");
            }
            else
            {
                db.Products.Add(product);
                db.SaveChanges();
                return product;
            }
        }

        public bool RemoveProduct(int id)
        {
            Product? db_product = db.Products.FirstOrDefault(u => u.Id == id);
            if (db_product == null)
            {
                throw new InvalidOperationException($"Product with id {id} does not exist");
            }
            db.Products.Remove(db_product);
            db.SaveChanges();
            return true;
        }

        public bool UpdateStock(int id, int stock)
        {
            if(stock < 0)
            {
                throw new ArgumentException("Stock cannot be less than 0");
            }
            Product? db_product = db.Products.FirstOrDefault(u => u.Id == id);
            if (db_product == null)
            {
                throw new InvalidOperationException($"Product with id {id} does not exist");
            }
            db_product.Stock = stock;
            db.SaveChanges();
            return true;
        }
    }
}
