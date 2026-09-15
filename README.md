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

## Running instructions

1. Install Visual Studio 2022 with the .NET desktop development workload.
2. Open `CommunityLibrary.sln` in Visual Studio.
3. Restore the NuGet packages when prompted.
4. Build the solution.
5. Run the project with the Visual Studio Start button.

## References and tools used

The project follows the approved project proposal and the ITS203 assessment requirements. Microsoft Learn and SQLite documentation are used as technical references during development.

Significant external assistance used during development should be acknowledged in the project documentation as required by the assessment brief.
