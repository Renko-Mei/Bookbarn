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
    }
