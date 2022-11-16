using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly Actions GetProductsAccess = Actions.Cart;
        private readonly Actions AddProductAccess = Actions.Cart;
        private readonly Actions RemoveProductAccess = Actions.Cart;
        private readonly Actions ClearCartAccess = Actions.Cart;

        private readonly SecurityService _securityService;
        private readonly Cart _cart;
        private readonly IUserRepository _userRepository;

        public CartController(SecurityService securityService, Cart cart, IUserRepository userRepository)
        {
            _securityService = securityService;
            _cart = cart;
            _userRepository = userRepository;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetProducts(Guid userId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetProductsAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? NotFound() : Ok(await _cart.GetUserProducts(userId));
        }

        [HttpPut("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> AddProduct(Guid userId, Guid productId, int quantity)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), AddProductAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");

            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь не найден");

                await _cart.AddProduct(userId, productId, quantity);
                await _userRepository.SaveAsync(user);
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> RemoveProduct(Guid userId, Guid productId, int? quantity = null)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), RemoveProductAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");

            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь не найден");

                await _cart.RemoveProduct(userId, productId, quantity);
                await _userRepository.SaveAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Удален из корзины");
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), ClearCartAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь не найден");
                await _cart.Clear(userId);
                await _userRepository.SaveAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Корзина очищена");
        }
    }
}
