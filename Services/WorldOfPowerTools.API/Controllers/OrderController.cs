using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly SaleService _saleService;
        private readonly Cart _cart;

        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderController(Cart cart, SaleService saleService, IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _cart = cart;
            _saleService = saleService;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            return Ok(await _orderRepository.GetAllAsync(skip, count));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _orderRepository.GetByIdAsync(id));
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOrders(Guid userId)
        {
            return Ok(await _orderRepository.GetByUserIdAsync(userId));
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrder(Guid userId, [FromQuery] Address address, [FromQuery] ContactData contactData)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь не найден");
                var cartProduct = await _cart.GetUserProducts(userId);
                if (!cartProduct.Any()) throw new OrderCouldNotBeCreatedException("Корзина пользователя пуста");
                var order = await _saleService.CreateOrder(userId, cartProduct, address, contactData);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                await _saleService.CancelOrder(orderId);
                return Ok("Заказ отменен");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendOrder(Guid orderId)
        {
            try
            {
                return Ok(await _saleService.SendOrder(orderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmOrder(Guid orderId)
        {
            try
            {
                return Ok(await _saleService.ConfirmOrder(orderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("delive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeliveOrder(Guid orderId)
        {
            try
            {
                return Ok(await _saleService.DeliveOrder(orderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("received")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReceivedOrder(Guid orderId)
        {
            try
            {
                return Ok(await _saleService.ReceivedOrder(orderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
