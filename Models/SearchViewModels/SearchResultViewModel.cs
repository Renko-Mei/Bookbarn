using System;
using System.Collections.Generic;

namespace BookBarn.Models.SearchViewModels
{
    public class SearchResultViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Quality { get; set; }
        public float Price { get; set; }
        public int SaleItemID { get; set; }
    }
}
