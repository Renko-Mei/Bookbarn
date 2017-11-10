using Microsoft.EntityFrameworkCore;

namespace final_project.Models
{
    public class final_projectContext : DbContext
    {
        public final_projectContext(DbContextOptions<final_projectContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //DbSets and classes generated by reverse-engineering
        }

        public DbSet<User> User { get; set; }
    }
}

// dotnet aspnet-codegenerator controller -name UsersController -m User -dc final_projectContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
