using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

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
            Product product = new Product();
            product.Name = name;
            product.Price = price;
            product.Stock = stock;
            product.Category = category;

            Product? db_product = db.Products.FirstOrDefault(u => u.Name == name);
            if(db_product != null)
            {
                return null;
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
            Product? db_product = db.Products.FirstOrDefault(u=>u.Id == id);
            if(db_product == null)
            {                
                return false;
            }
            db.Products.Remove(db_product);
            db.SaveChanges();
            return true;
        }

        public bool UpdateStock(int id, int stock)
        {
            Product? db_product = db.Products.FirstOrDefault(u => u.Id == id);
            if(db_product == null)
            {
                return false;
            }
            if(stock < 0)
            {
                return false;
            }
            db_product.Stock = stock;
            db.SaveChanges();
            return true;
        }
    }
}
