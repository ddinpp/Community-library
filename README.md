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

Day 4 completes the book-record stage. Book details are entered through the Windows Forms interface, checked before saving, stored in the local SQLite database and displayed in the records table. Saved records remain available when the application is opened again, and the Refresh Records button reloads the current database records. The project structure and existing member and loan classes are retained for future extension.

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
