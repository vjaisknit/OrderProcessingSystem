using Domain.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.InMemory;
using Persistence.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Application.BackgroundWorkers
{
    public class OrderQueueProcessor : BackgroundService
    {
        private readonly ILogger<OrderQueueProcessor> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderQueueProcessor(ILogger<OrderQueueProcessor> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderQueueProcessor started");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (InMemoryOrderQueue.PendingOrdersQueue.TryDequeue(out Order? order))
                {
                    try
                    {
                        order.OrderStatus = OrderStatus.Processing.ToString();
                        await Task.Delay(500, stoppingToken); // simulate processing
                        order.OrderStatus = OrderStatus.Completed.ToString();

                        using var scope = _scopeFactory.CreateScope();
                        var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                        await orderRepo.AddOrder(order);

                        _logger.LogInformation($"Order {order.Id} processed and saved to database.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing order {order.Id}");
                    }
                }

                await Task.Delay(100, stoppingToken);
            }

            _logger.LogInformation("OrderQueueProcessor stopped");
        }
    }
}
