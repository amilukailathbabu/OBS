using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OBS.Models;

public class Book
{
    public int BookId { get; set; }
    public string? Title { get; set; }
    public required string Author { get; set; }
    public string? Genre { get; set; }
   
   [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column(TypeName = "decimal(18, 2)")]  // Set precision and scale here
        public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Stock quantity must be at least 1")]
    public int StockQuantity { get; set; }

    public DateTime PublicationDate { get; set; }
    public required string Isbn { get; set; }
}