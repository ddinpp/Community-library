namespace CommunityLibrary.Models;

public class Book : LibraryItem
{
    public Book(int bookId, string isbn, string title, string author, string category, int totalCopies)
        : base(bookId, isbn, title, author, category, totalCopies)
    {
    }

    public override string GetItemType() => "Book";
}
