#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookMgtApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public int Age {get; set;}
    }
}