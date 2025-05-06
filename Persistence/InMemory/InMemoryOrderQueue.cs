using Domain.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.InMemory
{
    public static class InMemoryOrderQueue
    {
        // Queue only for pending orders (Created)
        public static ConcurrentQueue<Order> PendingOrdersQueue = new();
    }
}
