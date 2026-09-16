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

Day 2 adds a working book catalogue record feature. Book details can be entered through the Windows Forms interface, validated, saved to a local SQLite database and displayed in a records table. The records can also be refreshed after saving. The existing member and loan sections remain available for development in later stages.

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

## Running instructions

1. Open a terminal in the project folder.
2. Run `dotnet restore .\CommunityLibrary\CommunityLibrary.csproj`.
3. Run `dotnet build .\CommunityLibrary\CommunityLibrary.csproj`.
4. Run `dotnet run --project .\CommunityLibrary\CommunityLibrary.csproj`.
5. Enter book details and select **Save Book** to create a record.

The SQLite database file is created automatically in the application's output folder when the program starts.
