using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OBS.Data;
using OBS.Models;
using OBS.Services; 
namespace OBS.Controllers
{
    public class OrderController : Controller
    {
        private readonly OBSContext _context;
        private readonly OrderService _orderService; // Add OrderService as dependency

        public OrderController(OBSContext context, OrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var oBSContext = _context.Order.Include(o => o.Customer);
            return View(await oBSContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalAmount,ShippingAddress,OrderStatus")] Order order, List<OrderItem> orderItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure that the OrderItems list is not null or empty
                    if (orderItems != null && orderItems.Any())
                    {
                        // Using OrderService to create order and items
                        await _orderService.CreateOrderWithItemsAsync(
                            customerId: order.CustomerId,
                            totalAmount: order.TotalAmount,
                            shippingAddress: order.ShippingAddress,
                            orderItems: orderItems // Pass the order items here
                        );
                        return RedirectToAction(nameof(Index)); // Redirect to the Index view after success
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Order must contain at least one item.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception if any occurs
                    ModelState.AddModelError(string.Empty, "Error creating order: " + ex.Message);
                }
            }

            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,TotalAmount,ShippingAddress,OrderStatus")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}


















// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using OBS.Data;
// using OBS.Models;

// namespace OBS.Controllers
// {
//     public class OrderController : Controller
//     {
//         private readonly OBSContext _context;

//         public OrderController(OBSContext context)
//         {
//             _context = context;
//         }

//         // GET: Order
//         public async Task<IActionResult> Index()
//         {
//             var oBSContext = _context.Order.Include(o => o.Customer);
//             return View(await oBSContext.ToListAsync());
//         }

//         // GET: Order/Details/5
//         public async Task<IActionResult> Details(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }

//             var order = await _context.Order
//                 .Include(o => o.Customer)
//                 .FirstOrDefaultAsync(m => m.OrderId == id);
//             if (order == null)
//             {
//                 return NotFound();
//             }

//             return View(order);
//         }

//         // GET: Order/Create
//         public IActionResult Create()
//         {
//             ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
//             return View();
//         }

        

//         // GET: Order/Edit/5
//         public async Task<IActionResult> Edit(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }

//             var order = await _context.Order.FindAsync(id);
//             if (order == null)
//             {
//                 return NotFound();
//             }
//             ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
//             return View(order);
//         }

//         // POST: Order/Edit/5
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,TotalAmount,ShippingAddress,OrderStatus")] Order order)
//         {
//             if (id != order.OrderId)
//             {
//                 return NotFound();
//             }

//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(order);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if (!OrderExists(order.OrderId))
//                     {
//                         return NotFound();
//                     }
//                     else
//                     {
//                         throw;
//                     }
//                 }
//                 return RedirectToAction(nameof(Index));
//             }
//             ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
//             return View(order);
//         }

//         // GET: Order/Delete/5
//         public async Task<IActionResult> Delete(int? id)
//         {
//             if (id == null)
//             {
//                 return NotFound();
//             }

//             var order = await _context.Order
//                 .Include(o => o.Customer)
//                 .FirstOrDefaultAsync(m => m.OrderId == id);
//             if (order == null)
//             {
//                 return NotFound();
//             }

//             return View(order);
//         }

//         // POST: Order/Delete/5
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(int id)
//         {
//             var order = await _context.Order.FindAsync(id);
//             if (order != null)
//             {
//                 _context.Order.Remove(order);
//             }

//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }

//         private bool OrderExists(int id)
//         {
//             return _context.Order.Any(e => e.OrderId == id);
//         }


//         // POST: Order/Create
//         // To protect from overposting attacks, enable the specific properties you want to bind to.
//         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//         // [HttpPost]
//         // [ValidateAntiForgeryToken]
//         // public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalAmount,ShippingAddress,OrderStatus")] Order order)
//         // {
//         //     if (ModelState.IsValid)
//         //     {
//         //         _context.Add(order);
//         //         await _context.SaveChangesAsync();
//         //         return RedirectToAction(nameof(Index));
//         //     }
//         //     ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", order.CustomerId);
//         //     return View(order);
//         // }

//         [ApiController]
//         [Route("api/[controller]")]
//         public class OrderController : ControllerBase
//         {
//             private readonly OrderService _orderService;

//             public CreateOrder(OrderService orderService)
//             {
//                 _orderService = orderService;
//             }

//             [HttpPost("create")]
//             public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
//             {
//                 try
//                 {
//                     await _orderService.CreateOrderWithItemsAsync(
//                         customerId: request.CustomerId,
//                         totalAmount: request.TotalAmount,
//                         shippingAddress: request.ShippingAddress,
//                         orderItem: request.OrderItem
//                     );

//                     // return Ok(new { Message = "Order created successfully" });
//                     return View(order);
//                 }
//                 catch (Exception ex)
//                 {
//                     return StatusCode(500, new { Message = "Error creating order", Details = ex.Message });
//                 }
//             }
//         }


//     }
// }
