using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Web;

namespace BookBarn.Models.SearchViewModels
{
    public class SearchViewModel
    {
        public List<SearchResultViewModel> SearchResults { get; set; }
        public SelectList SearchTypes { get; set; }
        public string SearchType { get; set; }
        public SelectList SortTypes { get; set; }
        public string SortType { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string isbn { get; set; }
        public float minPrice { get; set; }
        public float maxPrice { get; set; }
    }
}
