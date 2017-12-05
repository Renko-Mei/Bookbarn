using System;
using System.Collections;
using System.Collections.Generic;
using BookBarn.Data;

namespace BookBarn.Models
{
  public class Order
  {
    public int OrderId { get; set; }
    public float SalePrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShippedDate { get; set; }
    public bool IsSold { get; set; }

    public List<SaleItem> SaleItems { get; set; }
    public String BuyerId { get; set; }
    public String SellerId { get; set; }
  }
}
