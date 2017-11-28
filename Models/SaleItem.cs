using BookBarn.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBarn.Models
{
    public class SaleItem
    {
        public int SaleItemId { get; set; }
        public float Price { get; set; }
        public bool IsSold { get; set; }
        public string Isbn { get; set; }
        public byte[] Image { get; set; }

        [Column("Quality")]
        public string QualityString
        {
            get { return Quality.ToString(); }
            private set { Quality = value.ParseEnum<BookQuality>(); }
        }

        public enum BookQuality
        {
            NEW,
            USED_LIKE_NEW,
            USED_OLD,
            USED_VERY_OLD
        }

        [NotMapped]
        public BookQuality Quality { get; set; }
    }
}