using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs.Wallet;
using AutoMapper;

namespace ECommerceAPI.Services
{
    public class WalletService:IWalletService
    {
        private AppDbContext db;
        private ILogger<TokenService> _logger;
        private IMapper mapper;
        private IValidationHelper _validationhelper;

        public WalletService(AppDbContext db, ILogger<TokenService> logger, IMapper mapper, IValidationHelper _validationhelper)
        {
            this.db = db;
            this._logger = logger;
            this.mapper = mapper;
            this._validationhelper = _validationhelper;
        }
        
        public void TopUp(WalletDTO walletDTO)
        {
            Wallet? wallet = db.Wallets.FirstOrDefault(u => u.customerId == walletDTO.customerId);
            if(wallet == null)
            {
                _logger.LogWarning($"Customer Id: {walletDTO.customerId} has no wallet");
                throw new ArgumentException($"Customer Id: {walletDTO.customerId} has no wallet");
            }
            _validationhelper.ValidateAmount(walletDTO.amount);
            wallet.amount += Math.Round(walletDTO.amount, 2, MidpointRounding.AwayFromZero);
            db.SaveChanges();
            _logger.LogInformation($"Customer Id: {walletDTO.customerId} has topped up ${walletDTO.amount}");
        }
        public void ChargeWallet(WalletDTO walletDTO)
        {
            Wallet? wallet = db.Wallets.FirstOrDefault(u => u.customerId == walletDTO.customerId);
            if (wallet == null)
            {
                _logger.LogWarning($"Customer Id: {walletDTO.customerId} has no wallet");
                throw new ArgumentException($"Customer Id: {walletDTO.customerId} has no wallet");
            }
            _validationhelper.ValidateAmount(walletDTO.amount);
            wallet.amount -= Math.Round(walletDTO.amount, 2, MidpointRounding.AwayFromZero);
            db.SaveChanges();
            _logger.LogInformation($"Customer Id: {walletDTO.customerId} has been charged ${walletDTO.amount}");
        }
        public void CreateWallet(int customerId)
        {
            User? db_user = db.Users.FirstOrDefault(u => u.Id == customerId);
            if (db_user == null)
            {
                _logger.LogWarning("User does not exist");
                throw new InvalidOperationException("User does not exist");
            }
            Wallet? db_wallet = db.Wallets.FirstOrDefault(u => u.customerId == customerId);
            if (db_wallet != null)
            {
                _logger.LogWarning("User has already created a wallet");
                throw new InvalidOperationException("User has already created a wallet");
            }
            Wallet wallet = new Wallet();
            wallet.customerId = customerId;
            wallet.amount = 0;
            db.Wallets.Add(wallet);
            db.SaveChanges();
            _logger.LogInformation($"Customer Id: {customerId} has created a wallet");
        }
        public WalletResponseDTO DisplayWallet(int customerId)
        {
            Wallet? wallet = db.Wallets.FirstOrDefault(u => u.customerId == customerId);
            if(wallet == null)
            {
                _logger.LogWarning($"Customer Id: {customerId} has no wallet");
                throw new ArgumentException($"Customer Id: {customerId} has no wallet");
            }
            WalletResponseDTO walletResponse = mapper.Map<WalletResponseDTO>(wallet);
            return walletResponse;
        }
    }
}
