using System;
using System.ComponentModel.DataAnnotations;

namespace BookBarn.Models
{
    public class SaleItem
    {
        [Key]
        public int SaleItemId { get; set; }
        public float Price { get; set; }
        public string Quality { get; set; }
        public bool IsSold { get; set; }
        public int BookId { get; set; }
        public byte[] Image { get; set; }
    }
}