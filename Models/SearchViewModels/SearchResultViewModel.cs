using System;
using System.Collections.Generic;

namespace BookBarn.Models.SearchViewModels
{
    public class SearchResultViewModel
    {
        public string Title { get; set; }
        public string Subtitle {get; set;}
        public string Author { get; set; }
        public string Quality { get; set; }
        public float Price { get; set; }
        public int SaleItemID { get; set; }
        public string ISBN { get; set; }
        public byte[] Image { get; set; }
        public bool IsSold { get; set; }
        public string UserKey {get; set;}
        public string Publisher { get; set; }
        public string PublishedDate {get; set;}
        public string Description {get; set;}
        public string Isbn10Or13 {get; set;}
        public string ImageLinks {get; set;}



    }
}
