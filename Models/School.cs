using System;

namespace final_project.Models
{
  public class School
  {
    public int ID { get; set; }
    public string Name { get; set; }
  }
}

// dotnet aspnet-codegenerator controller -name SchoolsController -m School -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
