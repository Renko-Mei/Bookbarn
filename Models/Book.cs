using System;
using System.ComponentModel.DataAnnotations;

namespace BookBarn.Models
{
    public class Book
    {
        [Key]
        [Display(Name = "ISBN")]
        [StringLength(14, MinimumLength = 10,
                ErrorMessage = "ISBN Must Be Between 10 and 13 characters long")]
        public string Isbn { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string Publisher { get; set; }
    }
}