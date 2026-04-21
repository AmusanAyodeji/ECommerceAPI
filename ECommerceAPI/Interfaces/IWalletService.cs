using ECommerceAPI.DTOs.Wallet;

namespace ECommerceAPI.Interfaces
{
    public interface IWalletService
    {
        public void CreateWallet(int customerId);
        public void TopUp(WalletDTO walletDTO);
        public void ChargeWallet(WalletDTO walletDTO);
        public WalletResponseDTO DisplayWallet(int customerId);
    }
}
