# Community Library Lending System

ITS203 Object-Oriented Design and Programming

A C# Windows Forms desktop application for managing books, members and lending activity in a small community library.

## Purpose

The project provides one place to maintain catalogue records, register members and manage book loans instead of keeping these records in separate notebooks or spreadsheets.

## Features

- Book catalogue records
- Library member records
- Lending transactions
- Book returns
- Customer view for browsing available books
- Customer view for checking a selected member's loans
- Overdue loan status checking
- Local SQLite data storage
- Input validation and exception handling
- Database transactions for lending and returns
- Refresh controls for current records

## Current progress

The final application contains the core library workflow. Books can be saved and displayed from SQLite, members can be registered and displayed, available books can be issued to active members with a due date, and active loans can be returned. Returning a loan restores the book's available-copy count. Active loans are checked against the current date when records are displayed so overdue loans can be identified. The interface includes separate Books, Members, Loans and Customer View tabs with validation and exception handling for user input and database operations.

The Customer View provides a simple member-facing part of the application. A member can be selected from the active member list, available books can be browsed, and that member's loan records can be viewed without exposing the administration controls used to add catalogue records or manage lending transactions.

The project uses an abstract LibraryItem base class, the Book subclass, Member and Loan models, a SQLite repository and a Windows Forms interface. Database transactions are used when issuing and returning books so the related loan and copy-count changes are committed together.

The final scope focuses on the working catalogue, member, customer-view and lending workflow rather than adding online accounts, reservations or payment features.

## Technology used

- C#
- .NET 8
- Windows Forms
- SQLite
- Visual Studio Code or Visual Studio
- Git and GitHub

## Project structure

```text
Community-library/
├── CommunityLibrary.sln
├── README.md
├── .gitignore
└── CommunityLibrary/
    ├── CommunityLibrary.csproj
    ├── Program.cs
    ├── Models/
    │   ├── LibraryItem.cs
    │   ├── Book.cs
    │   ├── Member.cs
    │   └── Loan.cs
    ├── Data/
    │   └── LibraryRepository.cs
    └── Forms/
        └── MainForm.cs
```
