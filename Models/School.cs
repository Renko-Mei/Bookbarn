using System;

namespace BookBarn.Models
{
  public class School
  {
    public int SchoolId { get; set; }
    public string Name { get; set; }
  }
}

// dotnet aspnet-codegenerator controller -name SchoolsController -m School -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
