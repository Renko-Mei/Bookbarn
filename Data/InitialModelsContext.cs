using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookBarn.Models;

namespace BookBarn.Data
{
    public class InitialModelsContext : DbContext
    {
        public InitialModelsContext(DbContextOptions<InitialModelsContext> options)
          : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<School> School { get; set; }

        public DbSet<SaleItem> SaleItem { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems {get; set; }
        

    }
}
