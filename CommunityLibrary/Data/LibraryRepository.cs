using CommunityLibrary.Models;
using Microsoft.Data.Sqlite;

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
            PRAGMA foreign_keys = ON;

            CREATE TABLE IF NOT EXISTS Books (
                BookId INTEGER PRIMARY KEY AUTOINCREMENT,
                ISBN TEXT NOT NULL,
                Title TEXT NOT NULL,
                Author TEXT NOT NULL,
                Category TEXT NOT NULL,
                TotalCopies INTEGER NOT NULL,
                AvailableCopies INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Members (
                MemberId INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                Phone TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                IsActive INTEGER NOT NULL DEFAULT 1
            );

            CREATE TABLE IF NOT EXISTS Loans (
                LoanId INTEGER PRIMARY KEY AUTOINCREMENT,
                BookId INTEGER NOT NULL,
                MemberId INTEGER NOT NULL,
                LoanDate TEXT NOT NULL,
                DueDate TEXT NOT NULL,
                ReturnDate TEXT NULL,
                Status TEXT NOT NULL,
                FOREIGN KEY (BookId) REFERENCES Books(BookId),
                FOREIGN KEY (MemberId) REFERENCES Members(MemberId)
            );
            """;
        command.ExecuteNonQuery();
    }

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (string.IsNullOrWhiteSpace(book.ISBN) ||
            string.IsNullOrWhiteSpace(book.Title) ||
            string.IsNullOrWhiteSpace(book.Author) ||
            string.IsNullOrWhiteSpace(book.Category))
        {
            throw new ArgumentException("Book details cannot be empty.");
        }

        if (book.TotalCopies < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(book.TotalCopies), "A book must have at least one copy.");
        }

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
            var book = new Book(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5));
            book.AvailableCopies = reader.GetInt32(6);
            books.Add(book);
        }
        return books;
    }

    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        if (string.IsNullOrWhiteSpace(member.FullName) ||
            string.IsNullOrWhiteSpace(member.Phone) ||
            string.IsNullOrWhiteSpace(member.Email))
        {
            throw new ArgumentException("Member details cannot be empty.");
        }

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Members (FullName, Phone, Email, IsActive) VALUES ($fullName, $phone, $email, $isActive);";
        command.Parameters.AddWithValue("$fullName", member.FullName);
        command.Parameters.AddWithValue("$phone", member.Phone);
        command.Parameters.AddWithValue("$email", member.Email);
        command.Parameters.AddWithValue("$isActive", member.IsActive ? 1 : 0);
        command.ExecuteNonQuery();
    }

    public List<Member> GetMembers()
    {
        var members = new List<Member>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT MemberId, FullName, Phone, Email, IsActive FROM Members ORDER BY FullName;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            members.Add(new Member(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4) == 1));
        }
        return members;
    }

    public void AddLoan(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);

        if (loan.DueDate.Date < loan.LoanDate.Date)
        {
            throw new InvalidOperationException("The due date cannot be before the loan date.");
        }

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            using var memberCommand = connection.CreateCommand();
            memberCommand.Transaction = transaction;
            memberCommand.CommandText = "SELECT IsActive FROM Members WHERE MemberId = $memberId;";
            memberCommand.Parameters.AddWithValue("$memberId", loan.MemberId);
            var memberResult = memberCommand.ExecuteScalar();
            if (memberResult is null)
            {
                throw new InvalidOperationException("The selected member does not exist.");
            }
            if (Convert.ToInt32(memberResult) != 1)
            {
                throw new InvalidOperationException("The selected member is not active.");
            }

            using var bookCommand = connection.CreateCommand();
            bookCommand.Transaction = transaction;
            bookCommand.CommandText = "SELECT AvailableCopies FROM Books WHERE BookId = $bookId;";
            bookCommand.Parameters.AddWithValue("$bookId", loan.BookId);
            var availableResult = bookCommand.ExecuteScalar();
            if (availableResult is null)
            {
                throw new InvalidOperationException("The selected book does not exist.");
            }
            if (Convert.ToInt32(availableResult) < 1)
            {
                throw new InvalidOperationException("The selected book has no available copies.");
            }

            using var insertCommand = connection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = "INSERT INTO Loans (BookId, MemberId, LoanDate, DueDate, ReturnDate, Status) VALUES ($bookId, $memberId, $loanDate, $dueDate, NULL, $status);";
            insertCommand.Parameters.AddWithValue("$bookId", loan.BookId);
            insertCommand.Parameters.AddWithValue("$memberId", loan.MemberId);
            insertCommand.Parameters.AddWithValue("$loanDate", loan.LoanDate.ToString("O"));
            insertCommand.Parameters.AddWithValue("$dueDate", loan.DueDate.ToString("O"));
            insertCommand.Parameters.AddWithValue("$status", LoanStatus.Active.ToString());
            insertCommand.ExecuteNonQuery();

            using var updateBookCommand = connection.CreateCommand();
            updateBookCommand.Transaction = transaction;
            updateBookCommand.CommandText = "UPDATE Books SET AvailableCopies = AvailableCopies - 1 WHERE BookId = $bookId;";
            updateBookCommand.Parameters.AddWithValue("$bookId", loan.BookId);
            updateBookCommand.ExecuteNonQuery();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void ReturnLoan(int loanId, DateTime returnDate)
    {
        if (loanId < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(loanId), "A valid loan must be selected.");
        }

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            using var loanCommand = connection.CreateCommand();
            loanCommand.Transaction = transaction;
            loanCommand.CommandText = "SELECT BookId, Status FROM Loans WHERE LoanId = $loanId;";
            loanCommand.Parameters.AddWithValue("$loanId", loanId);

            using var reader = loanCommand.ExecuteReader();
            if (!reader.Read())
            {
                throw new InvalidOperationException("The selected loan does not exist.");
            }

            var bookId = reader.GetInt32(0);
            var status = reader.GetString(1);
            if (!string.Equals(status, LoanStatus.Active.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The selected loan has already been returned.");
            }
            reader.Close();

            using var updateLoanCommand = connection.CreateCommand();
            updateLoanCommand.Transaction = transaction;
            updateLoanCommand.CommandText = "UPDATE Loans SET ReturnDate = $returnDate, Status = $status WHERE LoanId = $loanId;";
            updateLoanCommand.Parameters.AddWithValue("$returnDate", returnDate.ToString("O"));
            updateLoanCommand.Parameters.AddWithValue("$status", LoanStatus.Returned.ToString());
            updateLoanCommand.Parameters.AddWithValue("$loanId", loanId);
            updateLoanCommand.ExecuteNonQuery();

            using var updateBookCommand = connection.CreateCommand();
            updateBookCommand.Transaction = transaction;
            updateBookCommand.CommandText = "UPDATE Books SET AvailableCopies = AvailableCopies + 1 WHERE BookId = $bookId;";
            updateBookCommand.Parameters.AddWithValue("$bookId", bookId);
            updateBookCommand.ExecuteNonQuery();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public List<Loan> GetLoans()
    {
        var loans = new List<Loan>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT LoanId, BookId, MemberId, LoanDate, DueDate, ReturnDate, Status FROM Loans ORDER BY LoanId DESC;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var loan = new Loan(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), DateTime.Parse(reader.GetString(3)), DateTime.Parse(reader.GetString(4)));
            if (!reader.IsDBNull(5))
            {
                loan.ReturnDate = DateTime.Parse(reader.GetString(5));
            }
            loan.Status = Enum.TryParse<LoanStatus>(reader.GetString(6), true, out var status) ? status : LoanStatus.Active;
            loans.Add(loan);
        }
        return loans;
    }
}
