using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ViewModel
{
    public class OrderVM
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus OrderStatus { get; set; }
        public List<OrderItemVM> OrderItems { get; set; } = new List<OrderItemVM>();

    }

    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Refunded = 3
    }

    public enum PaymentMethod 
    {
        CreditCard = 0,
        PayPal = 1,
        BankTransfer = 2,
        CashOnDelivery = 3
    }

    public enum OrderStatus   
    {
        Pending = 0,
        Created = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4,
        Failed = 5
    }
}
