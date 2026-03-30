using System.IO;

namespace EquipmentRentalApp;

using System.Collections.Generic;
using System.IO;

public static class ConsoleBot
{
    public static bool RestoreMode { get; set; } = false;
    private static int _index = 0;

    private static List<string>? _inputs = null;

    public static string ReadStorageNext()
    {
        if (_inputs == null) LoadFromFile("./data.txt");
        
        if (_index >= _inputs.Count)
        {
            RestoreMode = false;
            return "welcome";
        }
        
        string value = _inputs[_index];
        _index++;

        return value;
    }
    
    public static void LoadFromFile(string path)
    {
        var lines = File.ReadAllLines(path);
        _inputs = new List<string>(lines);
    }
}