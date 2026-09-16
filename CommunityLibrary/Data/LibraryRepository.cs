using Microsoft.Data.Sqlite;
using CommunityLibrary.Models;

namespace CommunityLibrary.Data;

public class LibraryRepository
{
    public string DatabasePath { get; }

    public LibraryRepository(string databasePath)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            throw new ArgumentException("A database path is required.", nameof(databasePath));
        }

        DatabasePath = databasePath;
        InitialiseDatabase();
    }

    private string ConnectionString => $"Data Source={DatabasePath}";

    private void InitialiseDatabase()
    {
        var directory = Path.GetDirectoryName(DatabasePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Books (
                BookId INTEGER PRIMARY KEY AUTOINCREMENT,
                ISBN TEXT NOT NULL,
                Title TEXT NOT NULL,
                Author TEXT NOT NULL,
                Category TEXT NOT NULL,
                TotalCopies INTEGER NOT NULL,
                AvailableCopies INTEGER NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Books (ISBN, Title, Author, Category, TotalCopies, AvailableCopies)
            VALUES ($isbn, $title, $author, $category, $totalCopies, $availableCopies);
            """;
        command.Parameters.AddWithValue("$isbn", book.ISBN);
        command.Parameters.AddWithValue("$title", book.Title);
        command.Parameters.AddWithValue("$author", book.Author);
        command.Parameters.AddWithValue("$category", book.Category);
        command.Parameters.AddWithValue("$totalCopies", book.TotalCopies);
        command.Parameters.AddWithValue("$availableCopies", book.AvailableCopies);
        command.ExecuteNonQuery();
    }

    public List<Book> GetBooks()
    {
        var books = new List<Book>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT BookId, ISBN, Title, Author, Category, TotalCopies, AvailableCopies FROM Books ORDER BY Title;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var book = new Book(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetInt32(5));

            book.AvailableCopies = reader.GetInt32(6);
            books.Add(book);
        }

        return books;
    }
}
