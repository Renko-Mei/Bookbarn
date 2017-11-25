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
                        //new SaleItem
                        //{
                        //    Isbn = "0495012408",
                        //    Title = "Student Solutions Manual for Stewart's / Single Variable Calculus: Early Transcendentals",
                        //    Author = "James Stewart"
                        //},
                        //new SaleItem
                        //{
                        //    Isbn = "978-3-319-66966-3",
                        //    Title = "Introduction to Compiler Design",
                        //    Author = "Torben Ægidius Mogensen"
                        //},
                        //new SaleItem
                        //{
                        //    Isbn = "",
                        //    Title = "Metaprogramming Ruby 2: Program Like the Ruby Pros",
                        //    Author = "Paolo Perrotta"
                        //}
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
