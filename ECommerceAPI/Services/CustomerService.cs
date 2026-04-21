using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public class CustomerService:ICustomerService
    {
        private AppDbContext db;
        private ILogger<CustomerService> _logger;
        private IValidationHelper _validate;

        public CustomerService(AppDbContext db, ILogger<CustomerService> _logger, IValidationHelper _validate)
        {
            this.db = db;
            this._logger = _logger;
            this._validate = _validate;
        }

        public User GetById(int Id)
        {
            User? db_customer = db.Users.FirstOrDefault(u => u.Id == Id);
            _validate.CheckIfNull(db_customer, $"Customer with id: {Id} does not exist");
            return db_customer;
        }
    }
}
