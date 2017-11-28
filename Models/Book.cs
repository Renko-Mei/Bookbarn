using System;
using System.ComponentModel.DataAnnotations;

namespace BookBarn.Models
{
    public class Book
    {
        [Key]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string Publisher { get; set; }
    }
}