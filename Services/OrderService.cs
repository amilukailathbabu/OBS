using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using OBS.Models; // To access OrderItem and other models
using OBS.Data;   // To access OBSContext

namespace OBS.Services
{
    public class OrderService
    {
        private readonly OBSContext _context;

        public OrderService(OBSContext context)
        {
            _context = context;
        }

        public async Task CreateOrderWithItemsAsync(int customerId, decimal totalAmount, string shippingAddress, List<OrderItem> orderItems)
        {
            string orderInsertQuery = @"
        INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, ShippingAddress, OrderStatus) 
        OUTPUT INSERTED.OrderId 
        VALUES (@CustomerId, @OrderDate, @TotalAmount, @ShippingAddress, @OrderStatus)";

            string orderItemsInsertQuery = @"
        INSERT INTO OrderItems (OrderId, BookId, Quantity, PriceAtPurchase) 
        VALUES (@OrderId, @BookId, @Quantity, @PriceAtPurchase)";

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into Orders and get the OrderId
                        var orderId = await connection.ExecuteScalarAsync<int>(
                            orderInsertQuery,
                            new
                            {
                                CustomerId = customerId,
                                OrderDate = DateTime.Now,
                                TotalAmount = totalAmount,
                                ShippingAddress = shippingAddress,
                                OrderStatus = "Pending"
                            },
                            transaction
                        );

                        // Prepare OrderItems data
                        var orderItemsData = orderItems.Select(item => new
                        {
                            OrderId = orderId,
                            BookId = item.BookId,
                            Quantity = item.Quantity,
                            PriceAtPurchase = item.PriceAtPurchase
                        }).ToList();

                        // Insert into OrderItems
                        await connection.ExecuteAsync(
                            orderItemsInsertQuery,
                            orderItemsData,
                            transaction
                        );

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
