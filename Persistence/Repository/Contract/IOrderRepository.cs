using Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Contract
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<Order> GetOrderByIdAsync(string id);
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetAllOrdersAsync(int pageNumber, int pageSize);
    }
}
