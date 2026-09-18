using System.Drawing;
using CommunityLibrary.Data;
using CommunityLibrary.Models;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace CommunityLibrary.Forms;

public class MainForm : Form
{
    private readonly LibraryRepository repository;
    private readonly Label statusLabel;
    private readonly TextBox isbnTextBox;
    private readonly TextBox titleTextBox;
    private readonly TextBox authorTextBox;
    private readonly TextBox categoryTextBox;
    private readonly NumericUpDown copiesInput;
    private readonly DataGridView booksGrid;

    public MainForm()
    {
        Text = "Community Library Lending System";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 1000;
        Height = 650;
        MinimumSize = new Size(900, 600);

        var databasePath = Path.Combine(AppContext.BaseDirectory, "community-library.db");
        repository = new LibraryRepository(databasePath);

        var titleLabel = new Label
        {
            Text = "Community Library Lending System",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 25)
        };

        var infoLabel = new Label
        {
            Text = "Book catalogue",
            AutoSize = true,
            Location = new Point(33, 65)
        };

        isbnTextBox = CreateTextBox("ISBN", new Point(30, 105));
        titleTextBox = CreateTextBox("Title", new Point(190, 105));
        authorTextBox = CreateTextBox("Author", new Point(350, 105));
        categoryTextBox = CreateTextBox("Category", new Point(510, 105));

        copiesInput = new NumericUpDown
        {
            Location = new Point(670, 105),
            Width = 90,
            Minimum = 1,
            Maximum = 1000,
            Value = 1
        };

        var copiesLabel = new Label
        {
            Text = "Copies",
            AutoSize = true,
            Location = new Point(670, 85)
        };

        var saveButton = new Button
        {
            Text = "Save Book",
            Location = new Point(780, 103),
            Size = new Size(130, 30)
        };
        saveButton.Click += SaveBookButton_Click;

        booksGrid = new DataGridView
        {
            Location = new Point(30, 175),
            Size = new Size(910, 365),
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoGenerateColumns = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        var refreshButton = new Button
        {
            Text = "Refresh Records",
            Location = new Point(30, 555),
            Size = new Size(140, 32)
        };
        refreshButton.Click += (_, _) => LoadBooks();

        statusLabel = new Label
        {
            Text = "Enter the book details and select Save Book.",
            AutoSize = true,
            Location = new Point(190, 563)
        };

        Controls.Add(titleLabel);
        Controls.Add(infoLabel);
        Controls.Add(isbnTextBox);
        Controls.Add(titleTextBox);
        Controls.Add(authorTextBox);
        Controls.Add(categoryTextBox);
        Controls.Add(copiesLabel);
        Controls.Add(copiesInput);
        Controls.Add(saveButton);
        Controls.Add(booksGrid);
        Controls.Add(refreshButton);
        Controls.Add(statusLabel);

        LoadBooks();
    }

    private static TextBox CreateTextBox(string placeholder, Point location)
    {
        return new TextBox
        {
            PlaceholderText = placeholder,
            Location = location,
            Width = 145
        };
    }

    private void SaveBookButton_Click(object? sender, EventArgs e)
    {
        var isbn = isbnTextBox.Text.Trim();
        var title = titleTextBox.Text.Trim();
        var author = authorTextBox.Text.Trim();
        var category = categoryTextBox.Text.Trim();
        var copies = (int)copiesInput.Value;

        if (string.IsNullOrWhiteSpace(isbn) ||
            string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(author) ||
            string.IsNullOrWhiteSpace(category))
        {
            MessageBox.Show(
                "Please complete ISBN, title, author and category.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (isbn.Length > 20 ||
            title.Length > 150 ||
            author.Length > 100 ||
            category.Length > 80)
        {
            MessageBox.Show(
                "One or more fields are too long.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var book = new Book(0, isbn, title, author, category, copies);
            repository.AddBook(book);
            ClearBookInputs();
            LoadBooks();
            statusLabel.Text = "Book saved successfully.";
        }
        catch (SqliteException ex)
        {
            MessageBox.Show(
                $"The book could not be saved.

{ex.Message}",
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"The book could not be saved.

{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LoadBooks()
    {
        try
        {
            booksGrid.DataSource = repository.GetBooks()
                .Select(book => new
                {
                    book.BookId,
                    book.ISBN,
                    book.Title,
                    book.Author,
                    book.Category,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies
                })
                .ToList();

            statusLabel.Text = $"Records displayed: {booksGrid.Rows.Count}";
        }
        catch (SqliteException ex)
        {
            statusLabel.Text = "Unable to load book records.";
            MessageBox.Show(
                $"The book records could not be loaded.

{ex.Message}",
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            statusLabel.Text = "Unable to load book records.";
            MessageBox.Show(
                $"The book records could not be loaded.

{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ClearBookInputs()
    {
        isbnTextBox.Clear();
        titleTextBox.Clear();
        authorTextBox.Clear();
        categoryTextBox.Clear();
        copiesInput.Value = 1;
        isbnTextBox.Focus();
    }
}
