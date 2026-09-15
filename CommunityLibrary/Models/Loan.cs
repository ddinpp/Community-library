using System;

namespace CommunityLibrary.Models;

public enum LoanStatus
{
    Active,
    Returned,
    Overdue
}

public class Loan
{
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }

    public Loan(int loanId, int bookId, int memberId, DateTime loanDate, DateTime dueDate)
    {
        LoanId = loanId;
        BookId = bookId;
        MemberId = memberId;
        LoanDate = loanDate;
        DueDate = dueDate;
        Status = LoanStatus.Active;
    }

    public bool IsOverdue(DateTime today)
    {
        return Status == LoanStatus.Active && DueDate.Date < today.Date;
    }

    public void MarkReturned(DateTime returnDate)
    {
        ReturnDate = returnDate;
        Status = LoanStatus.Returned;
    }
}
