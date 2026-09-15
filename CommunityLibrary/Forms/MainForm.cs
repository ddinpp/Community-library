using System.Drawing;
using System.Windows.Forms;

namespace CommunityLibrary.Forms;

public class MainForm : Form
{
    private readonly Label statusLabel;

    public MainForm()
    {
        Text = "Community Library Lending System";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 900;
        Height = 560;

        var titleLabel = new Label
        {
            Text = "Community Library Lending System",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 25)
        };

        var infoLabel = new Label
        {
            Text = "Library catalogue and lending management",
            AutoSize = true,
            Location = new Point(33, 65)
        };

        var booksButton = new Button
        {
            Text = "Books",
            Location = new Point(30, 110),
            Size = new Size(150, 45)
        };

        var membersButton = new Button
        {
            Text = "Members",
            Location = new Point(195, 110),
            Size = new Size(150, 45)
        };

        var loansButton = new Button
        {
            Text = "Loans",
            Location = new Point(360, 110),
            Size = new Size(150, 45)
        };

        statusLabel = new Label
        {
            Text = "Select an option to continue.",
            AutoSize = true,
            Location = new Point(33, 185)
        };

        booksButton.Click += (_, _) => statusLabel.Text = "Book catalogue section selected.";
        membersButton.Click += (_, _) => statusLabel.Text = "Member management section selected.";
        loansButton.Click += (_, _) => statusLabel.Text = "Loan management section selected.";

        Controls.Add(titleLabel);
        Controls.Add(infoLabel);
        Controls.Add(booksButton);
        Controls.Add(membersButton);
        Controls.Add(loansButton);
        Controls.Add(statusLabel);
    }
}
