# Community Library Lending System

ITS203 Object-Oriented Design and Programming

A C# Windows Forms desktop application for managing books, members and lending activity in a small community library.

## Purpose

The project is designed for library staff or volunteers who need one place to manage catalogue records, members and book loans instead of keeping these records in separate notebooks or spreadsheets.

## Features

The starter project contains the core OOP model and project structure for:

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

The project foundation and main domain classes are in place. The Windows Forms user interface, database repository layer and transaction workflows will be developed in the next stages.

## Technology used

- C#
- .NET 8
- Windows Forms
- SQLite
- Visual Studio
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




