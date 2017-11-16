using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Web;

namespace final_project.Models
{
  public class SearchViewModel
  {
    public List<SearchResult> SearchResults { get; set; }
    public SelectList SearchTypes { get; set; }
    public string searchType { get; set; }
    //public List<string> Author { get; set; }
    //public List<string> Quality { get; set; }
    //public List<string> Price { get; set; }
    }
}
