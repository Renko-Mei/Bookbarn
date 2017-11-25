using System;
using System.ComponentModel.DataAnnotations;

namespace BookBarn.Models
{
    public class Book
    {
        public int BookId { get; set; }
        [Required]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string Publisher { get; set; }
    }
}