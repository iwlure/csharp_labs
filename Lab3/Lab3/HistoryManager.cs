using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Lab3;

public class History
{
    public History()
    {
    }

    public History(string key, List<double> values)
    {
        Key = key;
        Values = values;
    }

    [Key]
    public string Key { get; set; }
    [NotMapped]
    public List<double> Values { get; set; }

    public string InternalData { get; set; }
}

public class HistoryContext : DbContext
{
    public DbSet<History>? Histories { get; set; }

    private string DbPath { get; }

    public HistoryContext()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "history1.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<History>()
            .HasKey(h => h.Key);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}

public static class HistoryManager
{
    public static History UserHistory { get; private set; } = new("", new List<double>());

    public static void LoadHistory()
    {
        Console.WriteLine("Select the format to load history:");
        Console.WriteLine("1. JSON");
        Console.WriteLine("2. XML");
        Console.WriteLine("3. SQLite");
        var choice = GetUserChoice();

        switch (choice)
        {
            case 1:
                LoadJsonHistory();
                break;
            case 2:
                LoadXmlHistory();
                break;
            case 3:
                LoadSqLiteHistory();
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter a valid choice.");
                break;
        }
    }

    public static void SaveHistory()
    {
        Console.WriteLine("Select the format to save history:");
        Console.WriteLine("1. JSON");
        Console.WriteLine("2. XML");
        Console.WriteLine("3. SQLite");
        var choice = GetUserChoice();

        switch (choice)
        {
            case 1:
                SaveJsonHistory();
                break;
            case 2:
                SaveXmlHistory();
                break;
            case 3:
                SaveSqLiteHistory();
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter a valid choice.");
                break;
        }
    }

    private static void LoadJsonHistory()
    {
        Console.Write("Enter JSON file name to load history from: ");
        var fileName = Console.ReadLine();

        Console.Write("Enter key to load history: ");
        var key = Console.ReadLine();

        try
        {
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                var histories = JsonSerializer.Deserialize<List<History>>(json);

                var loadedHistory = histories?.FirstOrDefault(h => h.Key == key);

                if (loadedHistory != null)
                {
                    UserHistory = loadedHistory;
                    Console.WriteLine("History loaded successfully.");
                }
                else
                {
                    Console.WriteLine("History not found for the specified key.");
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading history: {ex.Message}");
        }
    }

    private static void SaveJsonHistory()
    {
        Console.Write("Enter JSON file name to save history to: ");
        var fileName = Console.ReadLine();

        Console.Write("Enter key to identify history: ");
        var key = Console.ReadLine();

        try
        {
            List<History> existingHistories;

            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                existingHistories = JsonSerializer.Deserialize<List<History>>(json) ?? new List<History>();
            }
            else
            {
                existingHistories = new List<History>();
            }

            UserHistory.Key = key ?? throw new ArgumentNullException(nameof(key));
            UserHistory.InternalData = string.Join(',', UserHistory.Values);

            existingHistories.Add(UserHistory);

            var updatedJson = JsonSerializer.Serialize(existingHistories);
            File.WriteAllText(fileName ?? throw new ArgumentNullException(nameof(fileName)), updatedJson);

            Console.WriteLine("History saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving history: {ex.Message}");
        }
    }

    private static void LoadXmlHistory()
    {
        Console.Write("Enter XML file name to load history from: ");
        var fileName = Console.ReadLine();

        Console.Write("Enter key to load history: ");
        var key = Console.ReadLine();

        try
        {
            var serializer = new XmlSerializer(typeof(List<History>));
            using var reader = new StreamReader(fileName ?? throw new ArgumentNullException(nameof(fileName)));
            var histories = serializer.Deserialize(reader) as List<History>;

            var loadedHistory = histories?.FirstOrDefault(h => h.Key == key);

            if (loadedHistory != null)
            {
                UserHistory = loadedHistory;
                Console.WriteLine("History loaded successfully.");
            }
            else
            {
                Console.WriteLine("History not found for the specified key.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading history: {ex.Message}");
        }
    }

    private static void SaveXmlHistory()
    {
        Console.Write("Enter XML file name to save history to: ");
        var fileName = Console.ReadLine();

        Console.Write("Enter key to identify history: ");
        var key = Console.ReadLine();

        try
        {
            var serializer = new XmlSerializer(typeof(List<History>));

            List<History> existingHistories;

            if (File.Exists(fileName))
            {
                using var reader = new StreamReader(fileName);
                existingHistories = serializer.Deserialize(reader) as List<History> ?? new List<History>();
            }
            else
            {
                existingHistories = new List<History>();
            }

            UserHistory.Key = key ?? throw new ArgumentNullException(nameof(key));
            UserHistory.InternalData = string.Join(',', UserHistory.Values);

            existingHistories.Add(UserHistory);

            using (var writer = new StreamWriter(fileName ?? throw new ArgumentNullException(nameof(fileName))))
            {
                serializer.Serialize(writer, existingHistories);
            }

            Console.WriteLine("History saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving history: {ex.Message}");
        }
    }


    private static void LoadSqLiteHistory()
    {
        Console.Write("Enter key to load history: ");
        var key = Console.ReadLine();

        try
        {
            using var context = new HistoryContext();
            context.Database.EnsureCreated();
            var history = context.Histories.FirstOrDefault(h => h.Key == key);
            if (history != null)
            {
                UserHistory = history;
                UserHistory.Values = UserHistory.InternalData.Split(',').Select(double.Parse).ToList();
                Console.WriteLine("History loaded successfully.");
            }
            else
            {
                Console.WriteLine("Key not found in the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading history: {ex.Message}");
        }
    }

    private static void SaveSqLiteHistory()
    {
        Console.Write("Enter key to save history: ");
        var key = Console.ReadLine();

        try
        {
            using var context = new HistoryContext();
            context.Database.EnsureCreated();
            var existingHistory = context.Histories.FirstOrDefault(h => h.Key == key);

            if (existingHistory != null)
            {
                Console.WriteLine("Key already exists in the database. Choose a different key.");
            }
            else
            {
                UserHistory.Key = key ?? throw new ArgumentNullException(nameof(key));
                UserHistory.InternalData = string.Join(',', UserHistory.Values);
                context.Histories.Add(UserHistory);
                context.SaveChanges();
                Console.WriteLine("History saved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving history: {ex.Message}");
        }
    }

    private static int GetUserChoice()
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }

        return choice;
    }
}