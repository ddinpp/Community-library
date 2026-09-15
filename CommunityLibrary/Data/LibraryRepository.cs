namespace CommunityLibrary.Data;

public class LibraryRepository
{
    public string DatabasePath { get; }

    public LibraryRepository(string databasePath)
    {
        DatabasePath = databasePath;
    }
}
