using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBarn.Models;

namespace BookBarn.Data
{
    public static class SeedSaleItems
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new InitialModelsContext(
                serviceProvider.GetRequiredService<DbContextOptions<InitialModelsContext>>()))
            {
                // Seed database if database is empty
                if (!context.SaleItem.Any())
                {
                    context.SaleItem.AddRange
                    (
                       new SaleItem
                       {
                           Isbn = "0495012408",
                           Quality = SaleItem.BookQuality.NEW,
                           Price = 10.6f,
                           Authors ="JK Rowling",
                           Title = "Harry P and Philosopher Stone",
                           UserKey = "abc"
                       },
                       new SaleItem
                       {
                           Isbn = "9783319669663",
                           Quality = SaleItem.BookQuality.USED_LIKE_NEW,
                           Price = 5.25f,
                           Authors ="JK Rowling",
                           Title = "Harry P and Chamber of Secret",
                           UserKey = "efg"
                       },
                       new SaleItem
                       {
                           Isbn = "9781941222126",
                           Quality = SaleItem.BookQuality.USED_OLD,
                           Price = 6.66f,
                           Authors ="JK Rowling",
                           Title = "Harry P and Prisoners of Azkaban",
                           UserKey = "abc"
                       }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
