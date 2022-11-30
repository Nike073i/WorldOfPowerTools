using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.API.RequestModels.Cart;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        public static readonly Actions GetProductsAccess = Actions.Cart;
        public static readonly Actions AddProductAccess = Actions.Cart;
        public static readonly Actions RemoveProductAccess = Actions.Cart;
        public static readonly Actions ClearCartAccess = Actions.Cart;

        private readonly SecurityService _securityService;
        private readonly Cart _cart;

        public CartController(SecurityService securityService, Cart cart)
        {
            _securityService = securityService;
            _cart = cart;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetProducts([Required] Guid userId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetProductsAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            return Ok(await _cart.GetUserProducts(userId));
        }

        [HttpPut("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]

        public async Task<IActionResult> AddProduct([FromBody] AddProductToCartModel model)
        {
            var userId = model.UserId;

            if (!_securityService.UserOperationAvailability(User.GetUserRights(), AddProductAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");

            var productId = model.ProductId;
            var quantity = model.Quantity;
            try
            {
                await _cart.AddProduct(userId, productId, quantity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Продукт добавлен в корзину");
        }

        [HttpDelete("remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductModel model)
        {
            var userId = model.UserId;
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), RemoveProductAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");

            var productId = model.ProductId;
            var quantity = model.Quantity;

            try
            {
                await _cart.RemoveProduct(userId, productId, quantity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Удален из корзины");
        }

        [HttpDelete("clear/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> ClearCart([Required] Guid userId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), ClearCartAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");

            try
            {
                await _cart.Clear(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Корзина очищена");
        }
    }
}
