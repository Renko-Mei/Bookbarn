using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Data
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

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<SaleItem>()
        //         .HasOne(b => b.Book)
        //         .WithMany(s => s.SaleItems)
        //         .HasForeignKey(s => s.BookId);
        // }
    }
}