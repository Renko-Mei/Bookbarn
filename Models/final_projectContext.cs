using Microsoft.EntityFrameworkCore;

namespace final_project.Models
{
  public class final_projectContext : DbContext
  {
    public final_projectContext (DbContextOptions<final_projectContext> options)
      : base(options)
    {
    }

    public DbSet<final_project.Models.User> User { get; set; }
  }
}

// dotnet aspnet-codegenerator controller -name UsersController -m User -dc final_projectContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
