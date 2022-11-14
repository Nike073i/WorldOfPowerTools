using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly SaleService _saleService;

        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository, SaleService saleService)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _saleService = saleService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            return Ok(await _orderRepository.GetAllAsync(skip, count));
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
        public async Task<IActionResult> CreateOrder(Guid userId, Address address, ContactData contactData)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return NotFound("Пользователь не найден");
                var order = await _saleService.CreateOrder(userId, user.GetCartProducts(), address, contactData);
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

        [HttpPost("send")]
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

        [HttpPost("confirm")]
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

        [HttpPost("delive")]
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

        [HttpPost("received")]
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
