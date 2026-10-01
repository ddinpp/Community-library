using System.Drawing;
using CommunityLibrary.Data;
using CommunityLibrary.Models;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace CommunityLibrary.Forms;

public class MainForm : Form
{
    private readonly LibraryRepository repository;

    private Label statusLabel = null!;
    private TextBox isbnTextBox = null!;
    private TextBox titleTextBox = null!;
    private TextBox authorTextBox = null!;
    private TextBox categoryTextBox = null!;
    private NumericUpDown copiesInput = null!;
    private DataGridView booksGrid = null!;

    private TextBox memberNameTextBox = null!;
    private TextBox memberPhoneTextBox = null!;
    private TextBox memberEmailTextBox = null!;
    private DataGridView membersGrid = null!;

    private ComboBox loanBookComboBox = null!;
    private ComboBox loanMemberComboBox = null!;
    private NumericUpDown loanDaysInput = null!;
    private DataGridView loansGrid = null!;

    private ComboBox customerMemberComboBox = null!;
    private DataGridView customerBooksGrid = null!;
    private DataGridView customerLoansGrid = null!;

    public MainForm()
    {
        Text = "Community Library Lending System";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 1100;
        Height = 720;
        MinimumSize = new Size(1000, 650);

        var databasePath = Path.Combine(AppContext.BaseDirectory, "community-library.db");
        repository = new LibraryRepository(databasePath);

        var titleLabel = new Label
        {
            Text = "Community Library Lending System",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 20)
        };

        var tabs = new TabControl
        {
            Location = new Point(20, 65),
            Size = new Size(1040, 570)
        };

        var booksTab = new TabPage("Books");
        BuildBooksTab(booksTab);

        var membersTab = new TabPage("Members");
        BuildMembersTab(membersTab);

        var loansTab = new TabPage("Loans");
        BuildLoansTab(loansTab);

        var customerTab = new TabPage("Customer View");
        BuildCustomerTab(customerTab);

        tabs.TabPages.Add(booksTab);
        tabs.TabPages.Add(membersTab);
        tabs.TabPages.Add(loansTab);
        tabs.TabPages.Add(customerTab);

        var refreshAllButton = new Button
        {
            Text = "Refresh All",
            Location = new Point(20, 650),
            Size = new Size(110, 32)
        };
        refreshAllButton.Click += (_, _) => RefreshAll();

        statusLabel = new Label
        {
            Text = "Ready.",
            AutoSize = true,
            Location = new Point(150, 658)
        };

        Controls.Add(titleLabel);
        Controls.Add(tabs);
        Controls.Add(refreshAllButton);
        Controls.Add(statusLabel);

        RefreshAll();
    }

    private void BuildBooksTab(TabPage tab)
    {
        tab.Controls.Add(new Label
        {
            Text = "Add a book to the catalogue.",
            AutoSize = true,
            Location = new Point(15, 15)
        });

        isbnTextBox = CreateTextBox("ISBN", new Point(15, 55), 150);
        titleTextBox = CreateTextBox("Title", new Point(180, 55), 150);
        authorTextBox = CreateTextBox("Author", new Point(345, 55), 150);
        categoryTextBox = CreateTextBox("Category", new Point(510, 55), 150);

        copiesInput = new NumericUpDown
        {
            Location = new Point(675, 55),
            Width = 80,
            Minimum = 1,
            Maximum = 1000,
            Value = 1
        };

        tab.Controls.Add(new Label
        {
            Text = "Copies",
            AutoSize = true,
            Location = new Point(675, 35)
        });

        var saveButton = new Button
        {
            Text = "Save Book",
            Location = new Point(770, 53),
            Size = new Size(120, 30)
        };
        saveButton.Click += SaveBookButton_Click;

        booksGrid = CreateGrid(new Point(15, 105), new Size(975, 355));

        var refreshButton = new Button
        {
            Text = "Refresh Books",
            Location = new Point(15, 475),
            Size = new Size(120, 30)
        };
        refreshButton.Click += (_, _) => LoadBooks();

        tab.Controls.Add(isbnTextBox);
        tab.Controls.Add(titleTextBox);
        tab.Controls.Add(authorTextBox);
        tab.Controls.Add(categoryTextBox);
        tab.Controls.Add(copiesInput);
        tab.Controls.Add(saveButton);
        tab.Controls.Add(booksGrid);
        tab.Controls.Add(refreshButton);
    }

    private void BuildMembersTab(TabPage tab)
    {
        tab.Controls.Add(new Label
        {
            Text = "Register and view library members.",
            AutoSize = true,
            Location = new Point(15, 15)
        });

        memberNameTextBox = CreateTextBox("Full name", new Point(15, 55), 230);
        memberPhoneTextBox = CreateTextBox("Phone", new Point(260, 55), 170);
        memberEmailTextBox = CreateTextBox("Email", new Point(445, 55), 250);

        var saveMemberButton = new Button
        {
            Text = "Save Member",
            Location = new Point(710, 53),
            Size = new Size(120, 30)
        };
        saveMemberButton.Click += SaveMemberButton_Click;

        membersGrid = CreateGrid(new Point(15, 105), new Size(975, 355));

        var refreshButton = new Button
        {
            Text = "Refresh Members",
            Location = new Point(15, 475),
            Size = new Size(130, 30)
        };
        refreshButton.Click += (_, _) => LoadMembers();

        tab.Controls.Add(memberNameTextBox);
        tab.Controls.Add(memberPhoneTextBox);
        tab.Controls.Add(memberEmailTextBox);
        tab.Controls.Add(saveMemberButton);
        tab.Controls.Add(membersGrid);
        tab.Controls.Add(refreshButton);
    }

    private void BuildLoansTab(TabPage tab)
    {
        tab.Controls.Add(new Label
        {
            Text = "Issue an available book to an active member.",
            AutoSize = true,
            Location = new Point(15, 15)
        });

        loanBookComboBox = new ComboBox
        {
            Location = new Point(15, 55),
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        loanMemberComboBox = new ComboBox
        {
            Location = new Point(330, 55),
            Width = 260,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        loanDaysInput = new NumericUpDown
        {
            Location = new Point(605, 55),
            Width = 80,
            Minimum = 1,
            Maximum = 60,
            Value = 14
        };

        tab.Controls.Add(new Label
        {
            Text = "Days",
            AutoSize = true,
            Location = new Point(605, 35)
        });

        var issueButton = new Button
        {
            Text = "Issue Book",
            Location = new Point(700, 53),
            Size = new Size(110, 30)
        };
        issueButton.Click += IssueBookButton_Click;

        var returnButton = new Button
        {
            Text = "Return Selected",
            Location = new Point(820, 53),
            Size = new Size(140, 30)
        };
        returnButton.Click += ReturnBookButton_Click;

        loansGrid = CreateGrid(new Point(15, 105), new Size(975, 355));

        var refreshButton = new Button
        {
            Text = "Refresh Loans",
            Location = new Point(15, 475),
            Size = new Size(120, 30)
        };
        refreshButton.Click += (_, _) => LoadLoans();

        tab.Controls.Add(loanBookComboBox);
        tab.Controls.Add(loanMemberComboBox);
        tab.Controls.Add(loanDaysInput);
        tab.Controls.Add(issueButton);
        tab.Controls.Add(returnButton);
        tab.Controls.Add(loansGrid);
        tab.Controls.Add(refreshButton);
    }

    private void BuildCustomerTab(TabPage tab)
    {
        tab.Controls.Add(new Label
        {
            Text = "Customer view: browse available books and check your loans.",
            AutoSize = true,
            Location = new Point(15, 15)
        });

        customerMemberComboBox = new ComboBox
        {
            Location = new Point(15, 55),
            Width = 280,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        customerMemberComboBox.SelectedIndexChanged += (_, _) => LoadCustomerLoans();

        tab.Controls.Add(new Label
        {
            Text = "Select member",
            AutoSize = true,
            Location = new Point(15, 35)
        });
        tab.Controls.Add(customerMemberComboBox);

        var refreshCustomerButton = new Button
        {
            Text = "Refresh",
            Location = new Point(310, 53),
            Size = new Size(100, 30)
        };
        refreshCustomerButton.Click += (_, _) =>
        {
            LoadCustomerBooks();
            LoadCustomerMembers();
        };
        tab.Controls.Add(refreshCustomerButton);

        tab.Controls.Add(new Label
        {
            Text = "Available books",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 105)
        });

        customerBooksGrid = CreateGrid(new Point(15, 130), new Size(975, 190));

        tab.Controls.Add(new Label
        {
            Text = "My loans",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 335)
        });

        customerLoansGrid = CreateGrid(new Point(15, 360), new Size(975, 150));

        tab.Controls.Add(customerBooksGrid);
        tab.Controls.Add(customerLoansGrid);
    }

    private static TextBox CreateTextBox(string placeholder, Point location, int width) => new()
    {
        PlaceholderText = placeholder,
        Location = location,
        Width = width
    };

    private static DataGridView CreateGrid(Point location, Size size) => new()
    {
        Location = location,
        Size = size,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AutoGenerateColumns = true,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };

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
            ShowValidation("Please complete ISBN, title, author and category.");
            return;
        }

        if (isbn.Length > 20 || title.Length > 150 || author.Length > 100 || category.Length > 80)
        {
            ShowValidation("One or more book fields are too long.");
            return;
        }

        try
        {
            repository.AddBook(new Book(0, isbn, title, author, category, copies));
            ClearBookInputs();
            LoadBooks();
            LoadLoanBooks();
            LoadCustomerBooks();
            statusLabel.Text = "Book saved successfully.";
        }
        catch (SqliteException ex)
        {
            ShowError("Database Error", "The book could not be saved.", ex);
        }
        catch (Exception ex)
        {
            ShowError("Error", "The book could not be saved.", ex);
        }
    }

    private void SaveMemberButton_Click(object? sender, EventArgs e)
    {
        var fullName = memberNameTextBox.Text.Trim();
        var phone = memberPhoneTextBox.Text.Trim();
        var email = memberEmailTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(email))
        {
            ShowValidation("Please complete name, phone and email.");
            return;
        }

        if (fullName.Length > 100 ||
            phone.Length > 30 ||
            email.Length > 150 ||
            !email.Contains('@'))
        {
            ShowValidation("Enter valid member details.");
            return;
        }

        try
        {
            repository.AddMember(new Member(0, fullName, phone, email));
            memberNameTextBox.Clear();
            memberPhoneTextBox.Clear();
            memberEmailTextBox.Clear();
            LoadMembers();
            LoadLoanMembers();
            LoadCustomerMembers();
            statusLabel.Text = "Member saved successfully.";
        }
        catch (SqliteException ex)
        {
            ShowError("Database Error", "The member could not be saved.", ex);
        }
        catch (Exception ex)
        {
            ShowError("Error", "The member could not be saved.", ex);
        }
    }

    private void IssueBookButton_Click(object? sender, EventArgs e)
    {
        if (loanBookComboBox.SelectedItem is not BookOption book ||
            loanMemberComboBox.SelectedItem is not MemberOption member)
        {
            ShowValidation("Select a book and an active member.");
            return;
        }

        try
        {
            var loanDate = DateTime.Now;
            var dueDate = loanDate.Date.AddDays((int)loanDaysInput.Value);

            repository.AddLoan(new Loan(0, book.BookId, member.MemberId, loanDate, dueDate));

            LoadBooks();
            LoadLoans();
            LoadLoanBooks();
            LoadCustomerBooks();
            LoadCustomerLoans();
            statusLabel.Text = $"Book issued to {member.FullName}.";
        }
        catch (InvalidOperationException ex)
        {
            ShowValidation(ex.Message);
        }
        catch (SqliteException ex)
        {
            ShowError("Database Error", "The loan could not be created.", ex);
        }
        catch (Exception ex)
        {
            ShowError("Error", "The loan could not be created.", ex);
        }
    }

    private void ReturnBookButton_Click(object? sender, EventArgs e)
    {
        if (loansGrid.CurrentRow?.Cells["LoanId"].Value is not int loanId)
        {
            ShowValidation("Select an active loan to return.");
            return;
        }

        try
        {
            repository.ReturnLoan(loanId, DateTime.Now);

            LoadBooks();
            LoadLoans();
            LoadLoanBooks();
            LoadCustomerBooks();
            LoadCustomerLoans();
            statusLabel.Text = "Book returned successfully.";
        }
        catch (InvalidOperationException ex)
        {
            ShowValidation(ex.Message);
        }
        catch (SqliteException ex)
        {
            ShowError("Database Error", "The book could not be returned.", ex);
        }
        catch (Exception ex)
        {
            ShowError("Error", "The book could not be returned.", ex);
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
                    book.TotalCopies,
                    book.AvailableCopies,
                    book.IsAvailable
                })
                .ToList();

            statusLabel.Text = $"Books displayed: {booksGrid.Rows.Count}";
        }
        catch (Exception ex)
        {
            ShowError("Error", "The book records could not be loaded.", ex);
        }
    }

    private void LoadMembers()
    {
        try
        {
            membersGrid.DataSource = repository.GetMembers()
                .Select(member => new
                {
                    member.MemberId,
                    member.FullName,
                    member.Phone,
                    member.Email,
                    member.IsActive
                })
                .ToList();

            statusLabel.Text = $"Members displayed: {membersGrid.Rows.Count}";
        }
        catch (Exception ex)
        {
            ShowError("Error", "The member records could not be loaded.", ex);
        }
    }

    private void LoadLoans()
    {
        try
        {
            loansGrid.DataSource = repository.GetLoans()
                .Select(loan => new
                {
                    loan.LoanId,
                    loan.BookId,
                    loan.MemberId,
                    loan.LoanDate,
                    loan.DueDate,
                    loan.ReturnDate,
                    Status = loan.IsOverdue(DateTime.Today) ? LoanStatus.Overdue : loan.Status
                })
                .ToList();

            statusLabel.Text = $"Loans displayed: {loansGrid.Rows.Count}";
        }
        catch (Exception ex)
        {
            ShowError("Error", "The loan records could not be loaded.", ex);
        }
    }

    private void LoadLoanBooks()
    {
        try
        {
            var selectedId = (loanBookComboBox.SelectedItem as BookOption)?.BookId;

            var options = repository.GetBooks()
                .Where(book => book.AvailableCopies > 0)
                .Select(book => new BookOption(
                    book.BookId,
                    $"{book.Title} ({book.AvailableCopies} available)"))
                .ToList();

            loanBookComboBox.DataSource = options;
            loanBookComboBox.DisplayMember = nameof(BookOption.DisplayText);
            loanBookComboBox.ValueMember = nameof(BookOption.BookId);

            if (selectedId.HasValue && options.Any(option => option.BookId == selectedId.Value))
            {
                loanBookComboBox.SelectedValue = selectedId.Value;
            }
        }
        catch (Exception ex)
        {
            ShowError("Error", "Available books could not be loaded.", ex);
        }
    }

    private void LoadLoanMembers()
    {
        try
        {
            var options = repository.GetMembers()
                .Where(member => member.IsActive)
                .Select(member => new MemberOption(member.MemberId, member.FullName))
                .ToList();

            loanMemberComboBox.DataSource = options;
            loanMemberComboBox.DisplayMember = nameof(MemberOption.DisplayText);
            loanMemberComboBox.ValueMember = nameof(MemberOption.MemberId);
        }
        catch (Exception ex)
        {
            ShowError("Error", "Active members could not be loaded.", ex);
        }
    }

    private void LoadCustomerBooks()
    {
        try
        {
            customerBooksGrid.DataSource = repository.GetBooks()
                .Where(book => book.AvailableCopies > 0)
                .Select(book => new
                {
                    book.Title,
                    book.Author,
                    book.Category,
                    AvailableCopies = book.AvailableCopies
                })
                .ToList();
        }
        catch (Exception ex)
        {
            ShowError("Error", "Available books could not be loaded.", ex);
        }
    }

    private void LoadCustomerMembers()
    {
        try
        {
            var selectedId = (customerMemberComboBox.SelectedItem as MemberOption)?.MemberId;

            var options = repository.GetMembers()
                .Where(member => member.IsActive)
                .Select(member => new MemberOption(member.MemberId, member.FullName))
                .ToList();

            customerMemberComboBox.DataSource = options;
            customerMemberComboBox.DisplayMember = nameof(MemberOption.DisplayText);
            customerMemberComboBox.ValueMember = nameof(MemberOption.MemberId);

            if (selectedId.HasValue && options.Any(option => option.MemberId == selectedId.Value))
            {
                customerMemberComboBox.SelectedValue = selectedId.Value;
            }
            else if (options.Count > 0)
            {
                customerMemberComboBox.SelectedIndex = 0;
            }

            LoadCustomerLoans();
        }
        catch (Exception ex)
        {
            ShowError("Error", "Customer members could not be loaded.", ex);
        }
    }

    private void LoadCustomerLoans()
    {
        if (customerLoansGrid is null)
        {
            return;
        }

        try
        {
            if (customerMemberComboBox.SelectedItem is not MemberOption member)
            {
                customerLoansGrid.DataSource = new List<object>();
                return;
            }

            customerLoansGrid.DataSource = repository.GetLoans()
                .Where(loan => loan.MemberId == member.MemberId)
                .Select(loan => new
                {
                    loan.LoanId,
                    loan.BookId,
                    loan.LoanDate,
                    loan.DueDate,
                    loan.ReturnDate,
                    Status = loan.IsOverdue(DateTime.Today) ? LoanStatus.Overdue : loan.Status
                })
                .ToList();
        }
        catch (Exception ex)
        {
            ShowError("Error", "Customer loan records could not be loaded.", ex);
        }
    }

    private void ClearBookInputs()
    {
        isbnTextBox.Clear();
        titleTextBox.Clear();
        authorTextBox.Clear();
        categoryTextBox.Clear();
        copiesInput.Value = 1;
    }

    private void RefreshAll()
    {
        LoadBooks();
        LoadMembers();
        LoadLoans();
        LoadLoanBooks();
        LoadLoanMembers();
        LoadCustomerBooks();
        LoadCustomerMembers();
    }

    private static void ShowValidation(string message)
    {
        MessageBox.Show(message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private static void ShowError(string title, string message, Exception exception)
    {
        MessageBox.Show($"{message}\n\n{exception.Message}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private sealed record BookOption(int BookId, string DisplayText);

    private sealed record MemberOption(int MemberId, string DisplayText);
}
