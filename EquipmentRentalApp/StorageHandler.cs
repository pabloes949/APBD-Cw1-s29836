using System.Text.Json;
namespace EquipmentRentalApp;

public static class StorageHandler
{
    public static JsonSerializerOptions SavingJsonOptions = new JsonSerializerOptions { WriteIndented = true };
    public static string DirPath { get; } = "./data";
    
    public enum OnError
    {
        Warn,
        Throw
    }

    public static void Load(string path, OnError action, Action<string> callback)
    {
        try
        {
            callback(File.ReadAllText(path));
        }
        catch (JsonException)
        {
            var err = new ConsoleException(28, new[] { path });
            if (action == OnError.Throw) throw err;
            if (action == OnError.Warn) Console.WriteLine($"WARN: {err.Message}");
        }
        catch (Exception e) when (
            e is DirectoryNotFoundException ||
            e is FileNotFoundException ||
            e is UnauthorizedAccessException ||
            e is IOException)
        {
            var err = new ConsoleException(27, new[] { path });
            if (action == OnError.Throw) throw err;
            if (action == OnError.Warn) Console.WriteLine($"WARN: {err.Message}");
        }
    }

    public static void Save(string path, OnError action, Func<string> callback)
    {
        try
        {
        EnsureFolderExists();
        File.WriteAllText(path, callback());
        }
        catch (Exception e)
        {
            var err = new ConsoleException(29, new[] { path });
            if(action == OnError.Throw) throw err;
            if (action == OnError.Warn) Console.WriteLine($"WARN: {err.Message}");
        }
    }

    private static void EnsureFolderExists()
    {
        Directory.CreateDirectory(DirPath);
    }

}