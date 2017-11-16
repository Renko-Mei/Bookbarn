using System;

namespace final_project.Models
{
  public class Book
  {
    public int BookId { get; set; }
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string AuthorFirstName { get; set; }
    public string AuthorLastName { get; set; }
  }
}

// dotnet aspnet-codegenerator controller -name BooksController -m Book -dc InitialModelsContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

