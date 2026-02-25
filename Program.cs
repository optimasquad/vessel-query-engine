using VesselQueryEngine.Core;
using VesselQueryEngine.Output;

namespace VesselQueryEngine;

/// <summary>
/// Main entry point for the Vessel Query Engine application
/// </summary>
class Program
{
    private static QueryEngine? _queryEngine;
    private static readonly HtmlExporter _htmlExporter = new();
    private static readonly JsonExporter _jsonExporter = new();
    private static readonly string _outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
    private static int _queryCounter = 0;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        PrintBanner();
        
        // Determine data file path
        string dataFilePath = args.Length > 0 ? args[0] : "/home/ubuntu/Uploads/vess.json";
        
        try
        {
            // Initialize the database
            InitializeDatabase(dataFilePath);
            
            // Create output directory
            Directory.CreateDirectory(_outputDirectory);
            
            // Start interactive mode
            RunInteractiveMode();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✘ Fatal Error: {ex.Message}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }

    private static void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╭─────────────────────────────────────────────────────╮
│   🚢 VESSEL QUERY ENGINE - In-Memory Database       │
│                                                     │
│   Version 1.0.0                                     │
│   Custom Query Language with AND Support           │
╰─────────────────────────────────────────────────────╯");
        Console.ResetColor();
    }

    private static void InitializeDatabase(string filePath)
    {
        Console.WriteLine($"\n⏳ Loading data from: {filePath}");
        
        var loader = new DataLoader();
        var vessels = loader.LoadFromFile(filePath);
        
        _queryEngine = new QueryEngine(vessels);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔ Database initialized successfully!");
        Console.WriteLine($"  • Total vessels loaded: {_queryEngine.GetTotalCount()}");
        Console.ResetColor();
    }

    private static void RunInteractiveMode()
    {
        var parser = new QueryParser();
        
        Console.WriteLine("\n" + new string('─', 55));
        Console.WriteLine("Type 'help' for query syntax, 'fields' to list fields,");
        Console.WriteLine("'exit' or 'quit' to close the application.");
        Console.WriteLine(new string('─', 55));
        
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("VesselDB> ");
            Console.ResetColor();
            
            string? input = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrWhiteSpace(input))
                continue;
            
            // Handle special commands
            switch (input.ToLower())
            {
                case "exit":
                case "quit":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n👋 Goodbye! Thank you for using Vessel Query Engine.");
                    Console.ResetColor();
                    return;
                    
                case "help":
                    Console.WriteLine(QueryParser.GetQueryHelp());
                    continue;
                    
                case "fields":
                    PrintAvailableFields();
                    continue;
                    
                case "clear":
                    Console.Clear();
                    PrintBanner();
                    continue;
            }
            
            // Parse and execute query
            var parsedQuery = parser.Parse(input);
            
            if (!parsedQuery.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✘ Parse Error: {parsedQuery.ErrorMessage}");
                Console.ResetColor();
                Console.WriteLine("   Type 'help' for query syntax examples.");
                continue;
            }
            
            // Execute query
            var result = _queryEngine!.Execute(parsedQuery);
            
            if (!result.Success)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✘ Execution Error: {result.ErrorMessage}");
                Console.ResetColor();
                continue;
            }
            
            // Display results summary
            DisplayResultsSummary(result);
            
            // Export results
            ExportResults(result);
        }
    }

    private static void PrintAvailableFields()
    {
        Console.WriteLine("\n┌─ Available Fields ──────────────────────────────────────┐");
        
        var fields = _queryEngine!.GetAvailableFields().OrderBy(f => f).ToList();
        int columns = 3;
        int rows = (int)Math.Ceiling(fields.Count / (double)columns);
        
        for (int i = 0; i < rows; i++)
        {
            Console.Write("│ ");
            for (int j = 0; j < columns; j++)
            {
                int index = i + j * rows;
                if (index < fields.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{fields[index],-25}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }
        
        Console.WriteLine("└──────────────────────────────────────────────────────────┘");
    }

    private static void DisplayResultsSummary(QueryResult result)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔ Query executed successfully!");
        Console.ResetColor();
        Console.WriteLine($"  • Total records in database: {result.TotalRecords}");
        Console.WriteLine($"  • Matching records: {result.MatchingRecords}");
        
        if (result.Results.Any())
        {
            Console.WriteLine("\n┌─── Top Results (max 10) ───────────────────────────────┐");
            
            int displayCount = Math.Min(10, result.Results.Count);
            for (int i = 0; i < displayCount; i++)
            {
                var vessel = result.Results[i];
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"│ {i + 1,2}. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{vessel.Z01_CURRENT_NAME,-25} ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"| {vessel.BUILDER_GROUP}");
                Console.ResetColor();
            }
            
            if (result.MatchingRecords > displayCount)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"│  ... and {result.MatchingRecords - displayCount} more");
                Console.ResetColor();
            }
            
            Console.WriteLine("└──────────────────────────────────────────────────────┘");
        }
    }

    private static void ExportResults(QueryResult result)
    {
        _queryCounter++;
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string baseName = $"query_{_queryCounter}_{timestamp}";
        
        // Export to HTML
        string htmlPath = Path.Combine(_outputDirectory, $"{baseName}.html");
        _htmlExporter.Export(result, htmlPath);
        
        // Export to JSON
        string jsonPath = Path.Combine(_outputDirectory, $"{baseName}.json");
        _jsonExporter.Export(result, jsonPath);
        
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("💾 Results exported to:");
        Console.ResetColor();
        Console.WriteLine($"  • HTML: {htmlPath}");
        Console.WriteLine($"  • JSON: {jsonPath}");
    }
}
