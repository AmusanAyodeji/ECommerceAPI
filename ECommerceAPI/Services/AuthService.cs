using ECommerceAPI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceAPI.Enums;
using ECommerceAPI.Models;
using ECommerceAPI.Interfaces;

namespace ECommerceAPI.Services
{
    public class AuthService:IAuthService
    {
        private AppDbContext db;

        public AuthService(AppDbContext db)
        {
            this.db = db;
        }
        public bool RegisterCustomer(string username, string password)
        {
            Customer customer = new Customer();
            customer.UserName = username;
            customer.Password = password;
            customer.Role = Roles.Customer;
            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (db_user != null)
            {
                return false;
            }
            else
            {
                db.Users.Add(customer);
                db.SaveChanges();
                return true;
            }

        }

        public bool RegisterAdmin(string username, string password)
        {
            Admin admin = new Admin();
            admin.UserName = username;
            admin.Password = password;
            admin.Role = Roles.Admin;

            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username);
            if(db_user != null)
            {
                return false;
            }
            else
            {
                db.Users.Add(admin);
                db.SaveChanges();
                return true;
            }
        }
        public User? Login(string username, string password)
        {
            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if(db_user == null)
            {
                return null;
            }
            else
            {
                return db_user;
            }
        } 
    }
}
