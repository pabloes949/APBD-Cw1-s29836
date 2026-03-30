namespace EquipmentRentalApp;

public static class RentalHandler
{
    private static Dictionary<Equipment, List<Rental>> EquipmentRentalHistory { get; } = new();
    private static Dictionary<string, Equipment> EquipmentIdentifiers { get; } = new();
    private static HashSet<Rental> RentalCollection { get; } = new();

    public static void RegisterNewEquipment(Equipment eq)
    {
        EquipmentRentalHistory.Add(eq, new List<Rental>());
        EquipmentIdentifiers.Add(eq.Id, eq);
    }

    public static void RegisterNewRental(Equipment equipment, Client client, Rental rental)
    {
        RentalCollection.Add(rental);
        RentalHandler.EquipmentRentalHistory[equipment].Add(rental);
        ClientHandler.ClientRentalHistory[client].Add(rental);
    }

    public static void RentEquipment(Equipment equipment, Client client, DateTime fromDate, DateTime toDate)
    {
        if (IsEquipmentRented(equipment)) throw new ConsoleException(1, new[] { equipment.Id, client.Id });
        int unpaidDays = ClientHandler.CountUnpaidDays(client);
        if (unpaidDays > 0) throw new ConsoleException(9, new[] { client.Id, unpaidDays.ToString() });
        if (toDate < fromDate) throw new ConsoleException(10, []);
        Rental rental = new Rental(equipment, client, fromDate, toDate);
        RentalHandler.RegisterNewRental(equipment, client, rental);
    }

    public static void ReturnEquipment(Equipment equipment)
    {
        if (!IsEquipmentRented(equipment)) throw new ConsoleException(2, new[] { equipment.Id });
        var history = EquipmentRentalHistory[equipment];
        var last = history[history.Count - 1];
        last.RegisterReturn();
    }
    
    public static void PayEquipment(Equipment equipment, Client client)
    {
        Rental? r = ClientHandler.HasClientUnpaidAsset(client, equipment);
        if(r == null) throw new ConsoleException(6, new[] { equipment.Id, client.Id });
        r.AcceptPayment();
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