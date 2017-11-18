using System;

namespace BookBarn.Models
{
    public class SaleItem
    {
        public int SaleItemId { get; set; }
        public float Price { get; set; }
        public string Quality { get; set; }
        public bool IsSold { get; set; }

        public int BookId { get; set; }
    }
}