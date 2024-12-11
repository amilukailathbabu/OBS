using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OBS.Models;


namespace OBS.Data
{
    public class OBSContext : DbContext
    {
        public  OBSContext (DbContextOptions<OBSContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Book { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        

        // public DbSet<OBS.Models.Book> Book { get; set; } = default!;
    }
}
