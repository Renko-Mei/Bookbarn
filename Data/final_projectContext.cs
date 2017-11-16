using Microsoft.EntityFrameworkCore;
using final_project.Models;

namespace final_project.Data
{
    public class final_projectContext : DbContext
    {
        public final_projectContext(DbContextOptions<final_projectContext> options)
          : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}

// dotnet aspnet-codegenerator controller -name UsersController -m User -dc final_projectContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
