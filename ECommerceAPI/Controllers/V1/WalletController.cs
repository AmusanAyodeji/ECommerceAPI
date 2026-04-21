using Asp.Versioning;
using ECommerceAPI.DTOs.Wallet;
using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IWalletService _walletService;

        public WalletController(IWalletService _walletService)
        {
            this._walletService = _walletService;
        }
        [HttpGet()]
        [Authorize]
        public IActionResult DisplayWallet(int customerId)
        {
            WalletResponseDTO wallet = _walletService.DisplayWallet(customerId);
            return Ok(wallet);
        }
        [HttpPatch()]
        [Authorize]
        public IActionResult TopUpWallet(WalletDTO walletDTO)
        {
            _walletService.TopUp(walletDTO);
            return Ok("Top Up Successful");
        }
    }
}
