using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBarn.Models;

namespace BookBarn.Data
{
    public class SeedSaleItems
    {

        private static readonly AuthenticationContext _Acontext;
        // public SeedSaleItems(AuthenticationContext Acontext){
        //     _Acontext = Acontext;
        // }
        // public string adminKey(){
        //     return _Acontext.Users.FirstOrDefault(c => c.UserName == "SuperAdmin").Id;
        // }

        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new InitialModelsContext(
                serviceProvider.GetRequiredService<DbContextOptions<InitialModelsContext>>()))
            {
                // Seed database if database is empty
                if (!context.SaleItem.Any())
                {
                    //var test = _Acontext.Users.FirstOrDefault(c => c.UserName == "SuperAdmin").Id;
                    context.SaleItem.AddRange
                    (
                       new SaleItem
                       {
                           Isbn = "1617292575",
                           Quality = SaleItem.BookQuality.NEW,
                           Price = 10.6f,
                           Authors ="Mike Cantelon",
                           Title = "Node.js in Action",
                           UserKey = "abc",
                           ImageLinks = "http://books.google.com/books/content?id=YzfuvQAACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
                           Publisher = "Manning Publications",
                           PublishedData = "2017-01-31",
                           Description = "test test test"
                       },
                       new SaleItem
                       {
                           Isbn = "1491901942",
                           Quality = SaleItem.BookQuality.USED_OLD,
                           Price = 10.6f,
                           Authors ="Shyam Seshadri",
                           Title = "AngularJS: Up and Running",
                           UserKey = "abc",
                           ImageLinks = "http://books.google.com/books/content?id=2BqloAEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api",
                           Publisher = "Oreilly & Associates Incorporated",
                           PublishedData = "2014-09-26",
                           Description = "A step-by-step guide to the AngularJS meta-framework covers from the basics to advanced concepts, including directives and controllers, form validation and stats, working with filters, unit testing, and guidelines and best practices."
                       },
                       new SaleItem
                       {
                           Isbn = "1593275846",
                           Quality = SaleItem.BookQuality.USED_VERY_OLD,
                           Price = 50.6f,
                           Authors ="Marijn Haverbeke",
                           Title = "Eloquent JavaScript, 2nd Ed.",
                           UserKey = "abc",
                           ImageLinks = "http://books.google.com/books/content?id=mDzDBQAAQBAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api",
                           Publisher = "No Starch Press",
                           PublishedData = "2014-12-14",
                           Description = "dsfgs sdfgkljk gkjsdfl kldfgkdfhla"
                       }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
