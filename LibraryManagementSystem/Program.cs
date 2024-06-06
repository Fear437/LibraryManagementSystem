using System.Text.Json;
namespace LibraryManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the library with books from a JSON file
        string fileName = "Library.json";
        string jsonString = File.ReadAllText(fileName);
        Library library;
        try {
            // Null forgiving operator (!) surpresses the null warning (we know json not null)
            // Deserialize the JSON string to a Library object
            library = JsonSerializer.Deserialize<Library>(jsonString)!;
            Console.WriteLine("Library loaded successfully.\n");
        } catch (JsonException) {
            Console.WriteLine("Error reading library from file. Creating a new library.\n");
            library = new();
        }
        
        // Receive commands from the user until exited
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Library Management System");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. View books");
            Console.WriteLine("3. Search for a book");
            Console.WriteLine("4. Borrow a book");
            Console.WriteLine("5. Return a book");
            Console.WriteLine("6. Exit");
            
            Console.WriteLine("Please enter a command: ");

            // String or null
            string? command = Console.ReadLine();
            Console.WriteLine();

            switch (command)
            {
                case "1":
                    library.AddBook();
                    break;
                case "2":
                    library.ViewBooks();
                    break;
                case "3":
                    library.SearchBook();
                    break;
                case "4":
                    library.BorrowBook();
                    break;
                case "5":
                    library.ReturnBook();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid command. Please try again.");
                    break;
            }
        }
    }
}

// Library class to represent a library
class Library
{
    public List<Book> Books { get; set; } = [];

    public void AddBook()
    {
        string title;
        string author;

        Console.WriteLine("Please enter the title of the book: ");
        do
        {
            title = Console.ReadLine()!;
        }
        while (string.IsNullOrEmpty(title));
        
        Console.WriteLine("Please enter the author of the book: ");
        do
        {
            author = Console.ReadLine()!;
        }
        while (string.IsNullOrEmpty(author));
        
        Books.Add(new Book(title, author));
        Console.WriteLine("Book added successfully.\n");
    }

    public void ViewBooks()
    {
        if (Books.Count == 0)
        {
            Console.WriteLine("No books in the library.\n");
            return;
        }

        foreach (var book in Books)
        {
            string status = book.IsBorrowed ? "Borrowed" : "Available";
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Status: {status}");
        }
        Console.WriteLine();
    }

// Search for a book by title or author
    public void SearchBook()
    {
        Console.WriteLine("Enter book title or author to search: ");

        // ?? is the null-coalescing operator, if null then empty string
        // empty string would print all books
        string searchQuery = Console.ReadLine() ?? "";

        // Find all books that contain the search query in the title or author
        var results = Books.FindAll(book => 
            book.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || 
            book.Author.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
        // searchQuery.ToLower(); // Should use ordinal ignore case

        if (results.Count == 0)
        {
            Console.WriteLine("No books found.\n");
        }
        else
        {
            // Print the search results
            foreach (var book in results)
            {
                string status = book.IsBorrowed ? "Borrowed" : "Available";
                Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Status: {status}");
            }
            Console.WriteLine();
        }
    }

    public void BorrowBook()
    {
        Console.WriteLine("Enter the title of the book to borrow: ");
        // TODO: Implement borrowing a book
    }

    public void ReturnBook()
    {
        Console.WriteLine("Enter the title of the book to return: ");
        // TODO: Implement returning a book
    }
}

// Book class to represent a book
class Book(string Title, string Author)
{
    public string Title { get; set; } = Title;
    public string Author { get; set; } = Author;
    public bool IsBorrowed { get; set; } = false;
}