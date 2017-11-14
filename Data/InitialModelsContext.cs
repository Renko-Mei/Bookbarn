using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using final_project.Models;

    public class InitialModelsContext : DbContext
    {
        public InitialModelsContext (DbContextOptions<InitialModelsContext> options)
          : base(options)
        {
        }

        public DbSet<final_project.Models.Book> Book { get; set; }

        public DbSet<final_project.Models.Address> Address { get; set; }

        public DbSet<final_project.Models.School> School { get; set; }

        public DbSet<final_project.Models.SaleItem> SaleItem { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<SaleItem>()
        //         .HasOne(b => b.Book)
        //         .WithMany(s => s.SaleItems)
        //         .HasForeignKey(s => s.BookId);
        // }
    }
