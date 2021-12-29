//#nullable enable
using System;
using System.ComponentModel.DataAnnotations;

namespace BookMgtApi.Models
{
    public class Book
    {

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }

        public string ISBN { get; set; }

        public DateTime PublishedDate { get; set; }

        // Foreign Key
        public int AuthorId { get; set; }
        // Navigation property

        [Required]
        public Author Author { get; set; }
    }
}