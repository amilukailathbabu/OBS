using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OBS.Models;

public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;

        // Navigation property for related OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Customer Customer { get; set; } // Navigation property for Customer
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public Book Book { get; set; }   // Navigation property for Book
        public Order Order { get; set; } // Navigation property for Order
    }

