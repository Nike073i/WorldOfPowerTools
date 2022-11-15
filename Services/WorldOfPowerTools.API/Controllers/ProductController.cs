using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductController(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int skip = 0, int? count = null)
        {
            return Ok(await _productRepository.GetAllAsync(skip, count));
        }

        [HttpGet("category")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCategory(Category category, int skip = 0, int? count = null)
        {
            return Ok(await _productRepository.GetByCategoryAsync(category, skip, count));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await _productRepository.GetByIdAsync(id) is { } item ? Ok(item) : NotFound();
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct(string name, double price, string description, int quantity,
            Category category = Category.Screwdriver, bool availability = true)
        {
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
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
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
        public async Task<IActionResult> AddProductToStore(Guid productId, int quantity)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) throw new Exception("Продукт по указанному Id не найден");
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
        public async Task<IActionResult> RemoveProductFromStore(Guid productId, int quantity)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) throw new Exception("Продукт по указанному Id не найден");
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
        public async Task<IActionResult> UpdateProduct(Guid productId, string? name = null, double? price = null, string? description = null, Category? category = null)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null) throw new Exception("Продукт по указанному Id не найден");
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
