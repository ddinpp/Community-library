# Community Library Lending System

ITS203 Object-Oriented Design and Programming

A C# Windows Forms desktop application for managing books, members and lending activity in a small community library.

## Purpose

The project is designed for library staff or volunteers who need one place to manage catalogue records, members and book loans instead of keeping these records in separate notebooks or spreadsheets.

## Features

- Book catalogue records
- Library member records
- Lending transactions
- Book returns
- Overdue loan checks
- Search and filtering
- Library summary information
- Local SQLite data storage
- Input validation and exception handling

## Current progress

The application now supports the main book catalogue workflow plus member and lending records. Books can be saved and displayed from SQLite, members can be registered and displayed, and available books can be issued to active members with a due date. Returning an active loan restores the book's available-copy count. The interface includes separate Books, Members and Loans tabs with refresh controls, and database transactions are used when issuing and returning books so related records stay consistent when an operation fails.

The project continues to use the existing model structure, including the abstract LibraryItem base class, the Book subclass, Member and Loan models, the SQLite repository and the Windows Forms interface.

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
