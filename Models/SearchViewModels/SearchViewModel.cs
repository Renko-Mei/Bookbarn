using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Web;

namespace final_project.Models.SearchViewModels
{
    public class SearchViewModel
    {
        public List<SearchResultViewModel> SearchResults { get; set; }
        public SelectList SearchTypes { get; set; }
        public string SearchType { get; set; }
    }
}
