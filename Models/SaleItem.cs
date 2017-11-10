using System;

namespace final_project.Models
{
  public class SaleItem
  {
    public int SaleItemID { get; set; }
    public int SellerID { get; set; }
    public string Price { get; set; }
    public string Quality { get; set; }
    public int BookID { get; set; }
    public bool isSold { get; set; }
    }
}
