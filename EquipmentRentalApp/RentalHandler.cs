using System.Text.Json;

namespace EquipmentRentalApp;

public static class RentalHandler
{
    private static Dictionary<Equipment, List<Rental>> EquipmentRentalHistory { get; } = new();
    private static Dictionary<string, Equipment> EquipmentIdentifiers { get; set; } = new();
    private static HashSet<Rental> RentalCollection { get; set; } = new();
    private static string RentalsUrl { get; } = Path.Combine(StorageHandler.DirPath, "rentals.json");
    private static string EquipmentUrl { get; } = Path.Combine(StorageHandler.DirPath, "equipment.json");
    
    public static void Load()
    {
        StorageHandler.Load(RentalsUrl, StorageHandler.OnError.Warn, (json) =>
            RentalCollection = JsonSerializer.Deserialize<HashSet<Rental>>(json) ?? RentalCollection);
        StorageHandler.Load(EquipmentUrl, StorageHandler.OnError.Warn, (json) =>
            EquipmentIdentifiers =
                JsonSerializer.Deserialize<Dictionary<string, Equipment>>(json) ?? EquipmentIdentifiers);
    }

    public static void RegisterNewEquipment(Equipment eq)
    {
        EquipmentRentalHistory.Add(eq, new List<Rental>());
        EquipmentIdentifiers.Add(eq.Id, eq);
        StorageHandler.Save(EquipmentUrl, StorageHandler.OnError.Warn, () =>
            JsonSerializer.Serialize(EquipmentIdentifiers, StorageHandler.SavingJsonOptions)
        );
    }

    public static void RegisterNewRental(Equipment equipment, Client client, Rental rental)
    {
        RentalCollection.Add(rental);
        RentalHandler.EquipmentRentalHistory[equipment].Add(rental);
        ClientHandler.ClientRentalHistory[client].Add(rental);

        StorageHandler.Save(RentalsUrl, StorageHandler.OnError.Warn, () =>
            JsonSerializer.Serialize(RentalCollection, StorageHandler.SavingJsonOptions)
        );
    }

    public static void RentEquipment(Equipment equipment, Client client, DateTime toDate)
    {
        if (IsEquipmentRented(equipment)) throw new ConsoleException(1, new[] { equipment.Id, client.Id });
        int unpaidDays = ClientHandler.CountUnpaidDays(client);
        if (unpaidDays > 0) throw new ConsoleException(9, new[] { client.Id, unpaidDays.ToString() });
        if (toDate < DateTime.Now) throw new ConsoleException(10, []);
        Rental rental = new Rental(equipment, client, toDate);
        RentalHandler.RegisterNewRental(equipment, client, rental);
    }

    public static void ReturnEquipment(Equipment equipment, Client client)
    {
        if (!IsEquipmentRented(equipment)) throw new ConsoleException(2, new[] { equipment.Id });
        var history = EquipmentRentalHistory[equipment];
        var last = history[history.Count - 1];
        last.RegisterReturn();
    }

    public static string GenerateEquipmentId()
    {
        string id;
        do id = Guid.NewGuid().ToString("N").Substring(0, 5); //5 digits
        while (EquipmentIdentifiers.ContainsKey(id));
        return id;
    }

    private static bool IsEquipmentRented(Equipment equipment)
    {
        List<Rental> history = EquipmentRentalHistory[equipment];
        Rental? last = history.Count > 0 ? history[history.Count - 1] : null;
        if (last == null) return false;
        return last.IsRented;
    }

    public static int GetEquipmentCount()
    {
        return EquipmentIdentifiers.Count;
    }

    public static void GetEquipmentList(Action<Equipment> callback)
    {
        foreach (Equipment eq in EquipmentIdentifiers.Values) callback(eq);
    }


    public static Equipment? GetEquipmentById(string id)
    {
        if (EquipmentIdentifiers.ContainsKey(id)) return EquipmentIdentifiers[id];
        return null;
    }
}