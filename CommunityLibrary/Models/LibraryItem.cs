using System;

namespace CommunityLibrary.Models;

public abstract class LibraryItem
{
    public int BookId { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    protected LibraryItem(int bookId, string isbn, string title, string author, string category, int totalCopies)
    {
        BookId = bookId;
        ISBN = isbn;
        Title = title;
        Author = author;
        Category = category;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
    }

    public bool IsAvailable => AvailableCopies > 0;

    public abstract string GetItemType();
}
