using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBarn.Models;

namespace BookBarn.Data
{
    public static class SeedBooks
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new InitialModelsContext(
                serviceProvider.GetRequiredService<DbContextOptions<InitialModelsContext>>()))
            {
                // Seed database if database is empty
                if (!context.Book.Any())
                {
                    context.Book.AddRange
                    (
                        new Book
                        {
                            Isbn = "0495012408",
                            Title = "Student Solutions Manual for Stewart's / Single Variable Calculus: Early Transcendentals",
                            Author = "James Stewart"
                        },
                        new Book
                        {
                            Isbn = "978-3-319-66966-3",
                            Title= "Introduction to Compiler Design",
                            Author = "Torben Ægidius Mogensen"
                        },
                        new Book
                        {
                            Isbn = "978-1-94122-212-6",
                            Title = "Metaprogramming Ruby 2: Program Like the Ruby Pros",
                            Author = "Paolo Perrotta"
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
