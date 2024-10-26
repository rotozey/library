using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Book
{
    public string Title;
    public string Author;
    public int Pages;

    public Book(string title, string author, int pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine("Книга: " + Title + ", Автор: " + Author + ", Страниц: " + Pages);
    }
}

class FictionBook : Book
{
    public FictionBook(string title, string author, int pages) : base(title, author, pages) { }

    public override void DisplayInfo()
    {
        Console.WriteLine("Художественная книга: " + Title + " - " + Author + ", " + Pages + " страниц.");
    }
}

class NonFictionBook : Book
{
    public NonFictionBook(string title, string author, int pages) : base(title, author, pages) { }

    public override void DisplayInfo()
    {
        Console.WriteLine("Документальная книга: " + Title + " - " + Author + ", " + Pages + " страниц.");
    }
}

class Library
{
    List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        books.Add(book);
        Console.WriteLine("Добавлена книга: " + book.Title);
    }

    public void RemoveBook(string title)
    {
        Book bookToRemove = null;
        foreach (var book in books)
        {
            if (book.Title == title)
            {
                bookToRemove = book;
                break;
            }
        }

        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            Console.WriteLine("Удалена книга: " + title);
        }
        else
        {
            Console.WriteLine("Книга не найдена.");
        }
    }

    public void FindBook(string title)
    {
        foreach (var book in books)
        {
            if (book.Title == title)
            {
                book.DisplayInfo();
                return;
            }
        }
        Console.WriteLine("Книга не найдена.");
    }

    public void DisplayAllBooks()
    {
        foreach (var book in books)
        {
            book.DisplayInfo();
        }
    }

    public void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter("library.txt"))
        {
            foreach (var book in books)
            {
                string line = book.GetType().Name + "," + book.Title + "," + book.Author + "," + book.Pages;
                writer.WriteLine(line);
            }
        }
        Console.WriteLine("Сохранено в файл.");
    }

    public void LoadFromFile()
    {
        if (File.Exists("library.txt"))
        {
            books.Clear();
            using (StreamReader reader = new StreamReader("library.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    Book book;
                    if (parts[0] == "FictionBook")
                    {
                        book = new FictionBook(parts[1], parts[2], int.Parse(parts[3]));
                    }
                    else
                    {
                        book = new NonFictionBook(parts[1], parts[2], int.Parse(parts[3]));
                    }
                    books.Add(book);
                }
            }
            Console.WriteLine("Загружено из файла.");
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Library library = new Library();

        while (true)
        {
            Console.WriteLine("Введите команду: добавить, удалить, найти, показать, сохранить, загрузить, выход");
            string command = Console.ReadLine();

            if (command == "добавить")
            {
                Console.Write("Введите название книги: ");
                string title = Console.ReadLine();
                Console.Write("Введите автора книги: ");
                string author = Console.ReadLine();
                Console.Write("Введите количество страниц: ");
                int pages = int.Parse(Console.ReadLine());
                Console.Write("Тип книги (художественная/документальная): ");
                string type = Console.ReadLine();

                Book book;
                if (type == "художественная")
                {
                    book = new FictionBook(title, author, pages);
                }
                else
                {
                    book = new NonFictionBook(title, author, pages);
                }
                library.AddBook(book);
            }
            else if (command == "удалить")
            {
                Console.Write("Введите название книги для удаления: ");
                string title = Console.ReadLine();
                library.RemoveBook(title);
            }
            else if (command == "найти")
            {
                Console.Write("Введите название книги для поиска: ");
                string title = Console.ReadLine();
                library.FindBook(title);
            }
            else if (command == "показать")
            {
                library.DisplayAllBooks();
            }
            else if (command == "сохранить")
            {
                library.SaveToFile();
            }
            else if (command == "загрузить")
            {
                library.LoadFromFile();
            }
            else if (command == "выход")
            {
                break;
            }
            else
            {
                Console.WriteLine("Неверная команда.");
            }
        }
    }
}
