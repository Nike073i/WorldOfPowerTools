﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public static readonly Actions AddProductAccess = Actions.Products;
        public static readonly Actions RemoveProductAccess = Actions.Products;
        public static readonly Actions LoadingProductAccess = Actions.Products;
        public static readonly Actions UnloadingProductAccess = Actions.Products;
        public static readonly Actions UpdateProductAccess = Actions.Products;

        private readonly SecurityService _securityService;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductController(SecurityService securityService, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _securityService = securityService;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            return Ok(await _productRepository.GetAllAsync(skip, count));
        }

        [HttpGet("category")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCategory([Required] Category category, int skip = 0, int? count = null)
        {
            return Ok(await _productRepository.GetByCategoryAsync(category, skip, count));
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            return await _productRepository.GetByIdAsync(id) is { } item ? Ok(item) : NotFound("Продукт по указанному Id не найден");
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> AddProduct([Required] string name, [Required] double price, [Required] string description, [Required] int quantity,
            Category category = Category.Screwdriver, bool availability = true)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), AddProductAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                var product = new Product(name, price, category, description, quantity, availability);
                var savedProduct = await _productRepository.SaveAsync(product);
                return Ok(savedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> RemoveProduct([Required] Guid id)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), RemoveProductAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                var createdOrders = await _orderRepository.GetByProductAndStatus(id, OrderStatus.Created);
                if (createdOrders.Any()) throw new EntityCouldNotBeRemovedException("Продукт не может быть удален, так существуют необработанные заказы с ним");
                var productId = await _productRepository.RemoveByIdAsync(id);
                return Ok(productId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("loading")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> AddProductToStore([Required] Guid productId, [Required] int quantity)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), LoadingProductAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) return NotFound("Продукт по указанному Id не найден");
                product.AddToStore(quantity);
                var changedProduct = await _productRepository.SaveAsync(product);
                return Ok(changedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("unloading")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> RemoveProductFromStore([Required] Guid productId, [Required] int quantity)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), UnloadingProductAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) return NotFound("Продукт по указанному Id не найден");
                product.RemoveFromStore(quantity);
                var changedProduct = await _productRepository.SaveAsync(product);
                return Ok(changedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> UpdateProduct([Required] Guid productId, string? name = null, double? price = null, string? description = null, Category? category = null)
        {
            if (!_securityService.UserOperationAvailability(User.GetUserRights(), UpdateProductAccess))
                return StatusCode(StatusCodes.Status405MethodNotAllowed, "У вас нет доступа к этой операции");
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) return NotFound("Продукт по указанному Id не найден");
                product.Name = name ?? product.Name;
                product.Price = price ?? product.Price;
                product.Description = description ?? product.Description;
                product.Category = category ?? product.Category;
                var changedProduct = await _productRepository.SaveAsync(product);
                return Ok(changedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
