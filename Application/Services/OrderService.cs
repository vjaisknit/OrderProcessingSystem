using Application.Common.Constants;
using Application.Services.Contract;
using AutoMapper;
using Domain.Data;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Persistence.InMemory;
using Persistence.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.HttpResponse;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IValidator<OrderVM> _orderValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IValidator<OrderVM> orderValidator,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderValidator = orderValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<Order>> CreateOrderAsync(OrderVM orderViewModel)
        {
            _logger.LogInformation("CreateOrderAsync started.");

            var validationResult = await _orderValidator.ValidateAsync(orderViewModel);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                _logger.LogWarning("Order validation failed: {@Errors}", errors);
                return ApiResponse<Order>.FailResponse(errors, StatusCodes.Status400BadRequest);
            }

            var order = _mapper.Map<Order>(orderViewModel);
            order.Id = Guid.NewGuid().ToString();

            InMemoryOrderQueue.PendingOrdersQueue.Enqueue(order);

            _logger.LogInformation("Order {OrderId} enqueued successfully.", order.Id);

            return ApiResponse<Order>.SuccessResponse(order, StatusCodes.Status200OK);
        }

        public async Task<ApiResponse<OrderVM>> GetOrderByIdAsync(string id)
        {
            _logger.LogInformation("Fetching order by ID: {OrderId}", id);

            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found.", id);
                return ApiResponse<OrderVM>.FailResponse("Order not found.", 404);
            }

            var orderVM = _mapper.Map<OrderVM>(order);

            _logger.LogInformation("Order {OrderId} retrieved successfully.", id);
            return ApiResponse<OrderVM>.SuccessResponse(orderVM);
        }

        public async Task<PaginatedResult<OrderVM>> GetPaginatedOrdersAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching paginated orders. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var (orders, totalCount) = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize);

            var result = new PaginatedResult<OrderVM>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = _mapper.Map<List<OrderVM>>(orders)
            };

            _logger.LogInformation("Paginated orders fetched. TotalCount: {TotalCount}", totalCount);

            return result;
        }
    }
}
