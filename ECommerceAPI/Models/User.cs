using System;
using System.Collections.Generic;
using System.Text;
using ECommerceAPI.Enums;

namespace ECommerceAPI.Models
{    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
