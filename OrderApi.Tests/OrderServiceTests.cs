using Application.Services;
using AutoMapper;
using Domain.Data;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace OrderApi.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IValidator<OrderVM>> _orderValidatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderValidatorMock = new Mock<IValidator<OrderVM>>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OrderService>>();

            _orderService = new OrderService(
                _orderRepositoryMock.Object,
                _orderValidatorMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnSuccess_WhenOrderIsValid()
        {
            // Arrange
            var orderVM = new OrderVM { CustomerName = "John" };
            var order = new Order { Id = "order-123" };

            _orderValidatorMock.Setup(v => v.ValidateAsync(orderVM, default))
                .ReturnsAsync(new ValidationResult());

            _mapperMock.Setup(m => m.Map<Order>(orderVM)).Returns(order);

            // Act
            var result = await _orderService.CreateOrderAsync(orderVM);

            // Assert
            Assert.True(result.StatusCode == (int) HttpStatusCode.OK);
            Assert.Equal(order.Id, result.Data.Id);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var orderVM = new OrderVM();
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure("CustomerName", "CustomerName is required")
        };

            _orderValidatorMock.Setup(v => v.ValidateAsync(orderVM, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            var result = await _orderService.CreateOrderAsync(orderVM);

            // Assert
            Assert.False(result.StatusCode == (int)HttpStatusCode.OK);
            Assert.Contains("CustomerName: CustomerName is required", result.Errors);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenExists()
        {
            // Arrange
            var order = new Order { Id = "order-123" };
            var orderVM = new OrderVM { Id = "order-123" };

            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync("order-123"))
                .ReturnsAsync(order);

            _mapperMock.Setup(m => m.Map<OrderVM>(order)).Returns(orderVM);

            // Act
            var result = await _orderService.GetOrderByIdAsync("order-123");

            // Assert
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
            Assert.Equal("order-123", result.Data.Id);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnFail_WhenNotFound()
        {
            // Arrange
            _orderRepositoryMock.Setup(r => r.GetOrderByIdAsync("not-found"))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync("not-found");

            // Assert
            Assert.False(result.StatusCode == (int)HttpStatusCode.OK);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetPaginatedOrdersAsync_ShouldReturnPaginatedResult()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { Id = "order-1" },
            new Order { Id = "order-2" }
        };

            var orderVMs = new List<OrderVM>
        {
            new OrderVM { Id = "order-1" },
            new OrderVM { Id = "order-2" }
        };

            _orderRepositoryMock.Setup(r => r.GetAllOrdersAsync(1, 2))
                .ReturnsAsync((orders, 2));

            _mapperMock.Setup(m => m.Map<List<OrderVM>>(orders)).Returns(orderVMs);

            // Act
            var result = await _orderService.GetPaginatedOrdersAsync(1, 2);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
        }
    }

}
