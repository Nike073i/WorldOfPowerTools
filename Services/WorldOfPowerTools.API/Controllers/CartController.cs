using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly Cart _cart;
        private readonly IUserRepository _userRepository;

        public CartController(Cart cart, IUserRepository userRepository)
        {
            _cart = cart;
            _userRepository = userRepository;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? NotFound() : Ok(await _cart.GetUserProducts(userId));
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
        public async Task<IActionResult> RemoveProduct(Guid userId, Guid productId, int? quantity = null)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Пользователь не найден");
            try
            {
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
        public async Task<IActionResult> RemoveProduct(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound("Пользователь не найден");
            try
            {
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
