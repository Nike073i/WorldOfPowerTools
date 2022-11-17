using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public static readonly Actions GetAllAccess = Actions.AllOrders;
        public static readonly Actions GetByIdAccess = Actions.AllOrders;
        public static readonly Actions GetMyOrderAccess = Actions.MyOrders;
        public static readonly Actions CreateOrderAccess = Actions.MyOrders;
        public static readonly Actions CancelOrderAccess = Actions.AllOrders;
        public static readonly Actions SendOrderAccess = Actions.AllOrders;
        public static readonly Actions ConfirmOrderAccess = Actions.AllOrders;
        public static readonly Actions DeliveOrderAccess = Actions.AllOrders;
        public static readonly Actions ReceiveOrderAccess = Actions.AllOrders;


        private readonly SecurityService _securityService;
        private readonly SaleService _saleService;
        private readonly Cart _cart;

        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderController(Cart cart, SaleService saleService, SecurityService securityService, IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _cart = cart;
            _saleService = saleService;
            _securityService = securityService;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetAllAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            return Ok(await _orderRepository.GetAllAsync(skip, count));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetByIdAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            return await _orderRepository.GetByIdAsync(id) is { } item ? Ok(item) : NotFound("Заказ по указанному Id не найден");
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> GetMyOrders([Required] Guid userId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), GetMyOrderAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            return Ok(await _orderRepository.GetByUserIdAsync(userId));
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> CreateOrder([Required] Guid userId, [FromQuery][Required] Address address, [FromQuery][Required] ContactData contactData)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), CreateOrderAccess) ||
                !_securityService.IsIndividualOperation(User.GetUserId(), userId))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> CancelOrder([Required] Guid orderId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), CancelOrderAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> SendOrder([Required] Guid orderId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), SendOrderAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> ConfirmOrder([Required] Guid orderId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), ConfirmOrderAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> DeliveOrder([Required] Guid orderId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), DeliveOrderAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> ReceiveOrder([Required] Guid orderId)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), ReceiveOrderAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                return Ok(await _saleService.ReceiveOrder(orderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
