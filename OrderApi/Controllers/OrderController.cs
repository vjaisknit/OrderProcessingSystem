using Application.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

namespace OrderApi.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost("create-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderVM orderVM)
        {
            var response = await _orderService.CreateOrderAsync(orderVM);

            if (response.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(response);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetPaginatedOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _orderService.GetPaginatedOrdersAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);

            if (response.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
