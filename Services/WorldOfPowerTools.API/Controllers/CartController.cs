using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public CartController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? NotFound() : Ok(user.GetCartProducts());
        }

        [HttpPut("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProduct(Guid userId, Guid productId, int quantity)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Пользователь не найден");
            try
            {
                user.AddProductInCart(productId, quantity);
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
        public async Task<IActionResult> RemoveProduct(Guid userId, Guid productId, int? quantity = null)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Пользователь не найден");
            try
            {
                user.RemoveProductFromCart(productId, quantity);
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
        public async Task<IActionResult> RemoveProduct(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Пользователь не найден");
            try
            {
                user.ClearCart();
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
