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
                           Price = 10.5f
                       },
                       new SaleItem
                       {
                           Isbn = "978-3-319-66966-3",
                           Quality = SaleItem.BookQuality.USED_LIKE_NEW,
                           Price = 5.25f
                       },
                       new SaleItem
                       {
                           Isbn = "978-1-94122-212-6",
                           Quality = SaleItem.BookQuality.USED_OLD,
                           Price = 6.66f
                       }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
