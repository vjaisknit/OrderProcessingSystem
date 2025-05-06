using Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.HttpResponse;

namespace Application.Services.Contract
{
    public interface IOrderService
    {
        Task<ApiResponse<Order>> CreateOrderAsync(OrderVM orderViewModel);
        Task<ApiResponse<OrderVM>> GetOrderByIdAsync(string id);
        Task<PaginatedResult<OrderVM>> GetPaginatedOrdersAsync(int pageNumber, int pageSize);
    }
}
