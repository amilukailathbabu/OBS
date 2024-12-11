using System.Collections.Generic;

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public List<OrderItemDto> OrderItem { get; set; }
}

public class OrderItemDto
{
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
}
