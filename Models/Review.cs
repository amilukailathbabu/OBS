using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OBS.Models;

public class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }

        public Customer Customer { get; set; } // Navigation property
        public Book Book { get; set; }         // Navigation property
    }

